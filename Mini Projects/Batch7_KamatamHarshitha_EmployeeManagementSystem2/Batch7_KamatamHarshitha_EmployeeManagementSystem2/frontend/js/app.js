$(document).ready(() => {

    // --- State Management ---
    let _state = {
        search: '',
        department: '',
        status: '',
        sortBy: 'name',
        sortDir: 'asc',
        page: 1,
        pageSize: PAGE_SIZE // from config.js
    };
    let _searchTimeout = null;

    // --- Routing & Initialization ---
    const checkAuthAndRoute = async () => {
        if (authService.isLoggedIn()) {
            $('#main-nav').removeClass('d-none');
            $('#login-view, #signup-view').addClass('d-none');

            const user = authService.getCurrentUser();
            $('#nav-username').text(user.charAt(0).toUpperCase() + user.slice(1));
            
            uiService.applyRoleUI();
            
            showView('dashboard');
            await refreshAppContent();
        } else {
            $('#main-nav').addClass('d-none');
            $('.view-section').addClass('d-none');
            $('#login-view').removeClass('d-none');
        }
    };

    const showView = (viewName) => {
        $('.view-section').addClass('d-none');
        $(`#${viewName}-view`).removeClass('d-none');
        $('.nav-link').removeClass('active');
        $(`#nav-${viewName}`).addClass('active');
        if(viewName === 'employees') {
             triggerFilterSortUpdate(); // refresh table when navigating to it
        }
    };

    const refreshAppContent = async () => {
        try {
            // Fetch dashboard data
            const summary = await dashboardService.getSummary();
            if (summary) {
                uiService.renderDashboardCards(summary);
                uiService.renderDepartmentBreakdown(summary.departmentBreakdown);
                uiService.renderRecentEmployees(summary.recentEmployees);
            }

            // The static departments are provided by the service
            uiService.populateDepartmentDropdown(employeeService.getUniqueDepartments());
            
            // Only update table if we are on the employees view to save API calls
            if(!$('#employees-view').hasClass('d-none')) {
                await triggerFilterSortUpdate();
            }
        } catch (error) {
            uiService.showToast('Error loading data', 'danger');
        }
    };

    const triggerFilterSortUpdate = async () => {
        try {
            const pagedResult = await employeeService.getAll(_state);
            uiService.renderEmployeeTable(pagedResult);
        } catch (error) {
            // If we get a 401 Unauthorized, the token probably expired. Force logout.
            if(error.message && error.message.includes('401')) {
                authService.logout();
                checkAuthAndRoute();
            }
        }
    };

    // --- Authentication Events ---
    $('#login-form').submit(async (e) => {
        e.preventDefault();
        const username = $('#login-username').val();
        const password = $('#login-password').val();

        const success = await authService.login(username, password);
        if (success) {
            $('#login-error').addClass('d-none');
            await checkAuthAndRoute();
            uiService.showToast('Login successful!');
        } else {
            $('#login-error').removeClass('d-none').text('Invalid credentials');
        }
    });

    $('#signup-form').submit(async (e) => {
        e.preventDefault();
        const u = $('#signup-username').val();
        const p = $('#signup-password').val();
        const c = $('#signup-confirm').val();
        const r = $('#signup-role').val(); // Ensure you add a role select to your signup form!

        const errors = validationService.validateAuthForm(u, p, c);
        uiService.showInlineErrors(errors);

        if (!errors) {
            const res = await authService.signup(u, p, r);
            if (res.success) {
                uiService.showToast('Signup successful. Please login.');
                $('#signup-view').addClass('d-none');
                $('#login-view').removeClass('d-none');
            } else {
                $('#err-signup-username').text(res.message).closest('.mb-3').find('input').addClass('is-invalid');
            }
        }
    });

    $('#logout-btn').click(() => {
        authService.logout();
        checkAuthAndRoute();
        uiService.clearForm('login-form');
    });

    $('#link-to-signup').click((e) => { e.preventDefault(); $('#login-view').addClass('d-none'); $('#signup-view').removeClass('d-none'); });
    $('#link-to-login').click((e) => { e.preventDefault(); $('#signup-view').addClass('d-none'); $('#login-view').removeClass('d-none'); });

    // --- Navigation Events ---
    $('#nav-dashboard').click((e) => { e.preventDefault(); showView('dashboard'); });
    $('#nav-employees').click((e) => { e.preventDefault(); showView('employees'); });
    $('.navbar-brand').click((e) => { e.preventDefault(); showView('dashboard'); });

    // --- Table Filtering, Sorting & Pagination Events ---
    $('#search-input').on('input', function() {
        clearTimeout(_searchTimeout);
        _searchTimeout = setTimeout(() => {
            _state.search = $(this).val();
            _state.page = 1; // reset to page 1 on search
            triggerFilterSortUpdate();
        }, 350); // 350ms debounce
    });

    $('#filter-dept').change(function() {
        _state.department = $(this).val();
        _state.page = 1;
        triggerFilterSortUpdate();
    });

    $('input[name="statusFilter"]').change(function() {
        _state.status = $(this).val();
        _state.page = 1;
        triggerFilterSortUpdate();
    });

    $('.sortable').click(function () {
        const field = $(this).data('sort');
        if (_state.sortBy === field) {
            _state.sortDir = _state.sortDir === 'asc' ? 'desc' : 'asc';
        } else {
            _state.sortBy = field;
            _state.sortDir = 'asc';
        }
        _state.page = 1;

        // Update UI icons
        $('.sortable i').removeClass('bi-arrow-up bi-arrow-down').addClass('bi-arrow-down-up text-muted');
        const icon = $(this).find('i');
        icon.removeClass('bi-arrow-down-up text-muted').addClass(_state.sortDir === 'asc' ? 'bi-arrow-up text-primary' : 'bi-arrow-down text-primary');

        triggerFilterSortUpdate();
    });

    // Pagination controls
    $('#btn-prev-page').click(() => {
        if (_state.page > 1) {
            _state.page--;
            triggerFilterSortUpdate();
        }
    });

    $('#btn-next-page').click(() => {
        _state.page++;
        triggerFilterSortUpdate();
    });

    // --- CRUD Events ---
    $('#nav-add-btn').click(() => uiService.showModal('add'));

    $('#save-employee-btn').click(async () => {
        const id = $('#emp-id').val();
        const isEdit = !!id;

        const data = {
            firstName: $('#emp-firstName').val(),
            lastName: $('#emp-lastName').val(),
            email: $('#emp-email').val(),
            phone: $('#emp-phone').val(),
            department: $('#emp-department').val(),
            designation: $('#emp-designation').val(),
            salary: Number($('#emp-salary').val()),
            joinDate: $('#emp-joinDate').val(),
            status: $('#emp-status').val()
        };

        // 1. Client-side validation
        const clientErrors = validationService.validateEmployeeForm(data);
        if (clientErrors) {
            uiService.showInlineErrors(clientErrors);
            return;
        }

        try {
            // 2. Server request
            if (isEdit) {
                await employeeService.update(parseInt(id), data);
                uiService.showToast('Employee updated successfully');
            } else {
                await employeeService.add(data);
                uiService.showToast('Employee added successfully');
                _state.page = 1; // Go to first page to see the new addition (due to default sort by ID desc ideally, or name)
            }
            uiService.closeModal('employeeModal');
            await refreshAppContent(); 
        } catch (error) {
            // 3. Map Server validation/conflict errors (like duplicate email)
            if (error.status === 400 || error.status === 409) {
                const mappedErrors = validationService.mapServerErrors(error.data);
                uiService.showInlineErrors(mappedErrors);
            } else {
                uiService.showToast('An unexpected error occurred.', 'danger');
            }
        }
    });

    $('#employee-table-body').on('click', '.btn-view', async function () {
        try {
            const id = $(this).data('id');
            const emp = await employeeService.getById(id);
            uiService.showModal('view', emp);
        } catch(e) {
             uiService.showToast('Failed to load employee details', 'danger');
        }
    });

    $('#employee-table-body').on('click', '.btn-edit', async function () {
        try {
            const id = $(this).data('id');
            const emp = await employeeService.getById(id);
            uiService.showModal('edit', emp);
        } catch(e) {
             uiService.showToast('Failed to load employee details', 'danger');
        }
    });

    $('#employee-table-body').on('click', '.btn-delete', async function () {
        try {
            const id = $(this).data('id');
            // Fetch name to show in confirmation dialog
            const emp = await employeeService.getById(id);
            uiService.showModal('delete', emp);
        } catch(e) {
            uiService.showToast('Failed to load employee details', 'danger');
        }
    });

    $('#confirm-delete-btn').click(async function () {
        try {
            const id = $(this).data('id');
            await employeeService.remove(id);
            uiService.closeModal('deleteModal');
            uiService.showToast('Employee deleted successfully', 'danger');
            
            // Re-fetch. If we deleted the last item on page 2, we might want to go to page 1.
            // For simplicity, we just trigger the update. The API could handle it or we check total count.
            await refreshAppContent();
        } catch (e) {
            uiService.showToast('Failed to delete employee.', 'danger');
        }
    });

    // --- Boot App ---
    checkAuthAndRoute();
});