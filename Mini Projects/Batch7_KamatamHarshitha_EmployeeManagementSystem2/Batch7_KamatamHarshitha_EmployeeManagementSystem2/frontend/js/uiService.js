const uiService = {
    formatCurrency: (amount) => {
        return new Intl.NumberFormat('en-IN', { style: 'currency', currency: 'INR', maximumFractionDigits: 0 }).format(amount);
    },
    getInitials: (fName, lName) => {
        const f = fName || '';
        const l = lName || '';
        return `${f.charAt(0)}${l.charAt(0)}`.toUpperCase();
    },
    getDeptColor: (dept) => {
        const map = { 'Engineering': 'primary', 'Marketing': 'warning text-dark', 'HR': 'info text-dark', 'Finance': 'success', 'Operations': 'secondary' };
        return map[dept] || 'dark';
    },
    
    renderDashboardCards: (summary) => {
        $('#kpi-total').text(summary.total || summary.Total || 0);
        $('#kpi-active').text(summary.active || summary.Active || 0);
        $('#kpi-inactive').text(summary.inactive || summary.Inactive || 0);
        $('#kpi-departments').text(summary.departments || summary.Departments || 0);
    },

    renderDepartmentBreakdown: (data) => {
        const container = $('#dept-breakdown-container');
        container.empty();
        if (!data) return;

        data.forEach(item => {
            const department = item.department || item.Department;
            const count = item.count || item.Count;
            const percentage = item.percentage !== undefined ? item.percentage : item.Percentage;

            const colorClass = uiService.getDeptColor(department);
            const barBgClass = colorClass.includes('text-dark') ? colorClass.replace(' text-dark', '') : colorClass;
            
            const html = `
                <div class="mb-3">
                    <div class="d-flex justify-content-between mb-1">
                        <span class="small fw-bold text-${colorClass}">${department}</span>
                        <span class="small text-muted">${count} (${percentage}%)</span>
                    </div>
                    <div class="progress" style="height: 6px;">
                        <div class="progress-bar bg-${barBgClass}" role="progressbar" style="width: ${percentage}%"></div>
                    </div>
                </div>`;
            container.append(html);
        });
    },

    renderRecentEmployees: (employees) => {
        const list = $('#recent-employees-list');
        list.empty();
        if (!employees) return;

        employees.forEach(emp => {
            const firstName = emp.firstName || emp.FirstName;
            const lastName = emp.lastName || emp.LastName;
            const designation = emp.designation || emp.Designation;
            const department = emp.department || emp.Department;
            const status = emp.status || emp.Status;

            const initial = uiService.getInitials(firstName, lastName);
            const statusClass = status === 'Active' ? 'success' : 'danger';
            const html = `
                <li class="list-group-item d-flex justify-content-between align-items-center py-3">
                    <div class="d-flex align-items-center">
                        <div class="avatar-circle bg-primary me-3">${initial}</div>
                        <div>
                            <h6 class="mb-0">${firstName} ${lastName}</h6>
                            <small class="text-muted">${designation}</small>
                        </div>
                    </div>
                    <div class="text-end">
                        <span class="badge bg-${uiService.getDeptColor(department)} mb-1 d-block">${department}</span>
                        <span class="badge bg-${statusClass}">${status}</span>
                    </div>
                </li>`;
            list.append(html);
        });
    },

    renderEmployeeTable: (pagedResult) => {
        const tbody = $('#employee-table-body');
        tbody.empty();
        
        // Safely extract the array and totals from PascalCase or camelCase
        const dataArray = pagedResult.data || pagedResult.Data || [];
        const totalCount = pagedResult.totalCount !== undefined ? pagedResult.totalCount : (pagedResult.TotalCount || 0);
        
        $('#showing-count-label').text(`Showing ${dataArray.length} of ${totalCount} employees`);

        if (dataArray.length === 0) {
            $('#empty-state').removeClass('d-none');
            tbody.closest('table').addClass('d-none');
            $('#pagination-container').addClass('d-none');
            return;
        }

        $('#empty-state').addClass('d-none');
        tbody.closest('table').removeClass('d-none');
        $('#pagination-container').removeClass('d-none');

        // Extract pagination info for calculating the row number sequence
        const page = pagedResult.page || pagedResult.Page || 1;
        const pageSize = pagedResult.pageSize || pagedResult.PageSize || 10;
        const startIndex = (page - 1) * pageSize;

        // Note the added 'index' parameter in the forEach loop
        dataArray.forEach((emp, index) => {
            const dbId = emp.id || emp.Id; // The real database ID needed for CRUD actions
            const displayRowNumber = startIndex + index + 1; // The calculated 1 to N row number
            
            const firstName = emp.firstName || emp.FirstName;
            const lastName = emp.lastName || emp.LastName;
            const email = emp.email || emp.Email;
            const department = emp.department || emp.Department;
            const designation = emp.designation || emp.Designation;
            const salary = emp.salary !== undefined ? emp.salary : emp.Salary;
            const joinDate = emp.joinDate || emp.JoinDate;
            const status = emp.status || emp.Status;

            const initial = uiService.getInitials(firstName, lastName);
            const statusBadge = status === 'Active' ? 'success' : 'danger';
            const deptColor = uiService.getDeptColor(department);
            
            const tr = `
                <tr>
                    <td class="align-middle text-muted">${displayRowNumber}</td>
                    <td class="align-middle">
                        <div class="avatar-circle bg-primary" style="width: 30px; height: 30px; font-size: 12px;">${initial}</div>
                    </td>
                    <td class="align-middle fw-bold">${firstName} ${lastName}</td>
                    <td class="align-middle text-muted small">${email}</td>
                    <td class="align-middle"><span class="badge bg-${deptColor}">${department}</span></td>
                    <td class="align-middle">${designation}</td>
                    <td class="align-middle">${uiService.formatCurrency(salary)}</td>
                    <td class="align-middle">${joinDate}</td>
                    <td class="align-middle"><span class="badge bg-${statusBadge}">${status}</span></td>
                    <td class="align-middle">
                        <button class="btn btn-sm btn-outline-info me-1 btn-view" data-id="${dbId}"><i class="bi bi-eye"></i></button>
                        <button class="btn btn-sm btn-outline-secondary me-1 btn-edit admin-only" data-id="${dbId}"><i class="bi bi-pencil"></i></button>
                        <button class="btn btn-sm btn-outline-danger btn-delete admin-only" data-id="${dbId}"><i class="bi bi-trash"></i></button>
                    </td>
                </tr>`;
            tbody.append(tr);
        });

        const totalPages = pagedResult.totalPages || pagedResult.TotalPages || 1;
        const hasPrev = pagedResult.hasPrevPage !== undefined ? pagedResult.hasPrevPage : pagedResult.HasPrevPage;
        const hasNext = pagedResult.hasNextPage !== undefined ? pagedResult.hasNextPage : pagedResult.HasNextPage;

        $('#page-info').text(`Page ${page} of ${totalPages}`);
        $('#btn-prev-page').prop('disabled', !hasPrev);
        $('#btn-next-page').prop('disabled', !hasNext);
        
        uiService.applyRoleUI();
    },
    
    applyRoleUI: () => {
        const isAdmin = authService.isAdmin();
        
        if (isAdmin) {
            $('.admin-only').removeClass('d-none');
            $('#viewer-notice').addClass('d-none');
            $('#nav-role-badge').text('Admin').removeClass('bg-secondary').addClass('bg-primary');
        } else {
            $('.admin-only').addClass('d-none');
            $('#viewer-notice').removeClass('d-none');
            $('#nav-role-badge').text('Viewer').removeClass('bg-primary').addClass('bg-secondary');
        }
    },

    populateDepartmentDropdown: (departments) => {
        const select = $('#filter-dept');
        select.find('option:not(:first)').remove();
        departments.forEach(d => select.append(`<option value="${d}">${d}</option>`));
    },

    showInlineErrors: (errors) => {
        $('.is-invalid').removeClass('is-invalid');
        $('.invalid-feedback').text('');

        if (errors) {
            Object.keys(errors).forEach(key => {
                const el = $(`#emp-${key}, #signup-${key}`);
                if(el.length) {
                    el.addClass('is-invalid');
                    el.siblings('.invalid-feedback').text(errors[key]);
                }
            });
        }
    },

    clearForm: (formId) => {
        $(`#${formId}`)[0].reset();
        $('#emp-id').val(''); 
        $('.is-invalid').removeClass('is-invalid');
    },

    populateForm: (emp) => {
        $('#emp-id').val(emp.id || emp.Id);
        $('#emp-firstName').val(emp.firstName || emp.FirstName);
        $('#emp-lastName').val(emp.lastName || emp.LastName);
        $('#emp-email').val(emp.email || emp.Email);
        $('#emp-phone').val(emp.phone || emp.Phone);
        $('#emp-department').val(emp.department || emp.Department);
        $('#emp-designation').val(emp.designation || emp.Designation);
        $('#emp-salary').val(emp.salary !== undefined ? emp.salary : emp.Salary);
        
        let dateVal = emp.joinDate || emp.JoinDate || '';
        if (dateVal && dateVal.includes('T')) dateVal = dateVal.split('T')[0];
        $('#emp-joinDate').val(dateVal);
        
        $('#emp-status').val(emp.status || emp.Status);
    },

    showModal: (type, data = null) => {
        if (type === 'add') {
            uiService.clearForm('employee-form');
            $('#employeeModalTitle').text('Add Employee');
            $('#save-employee-btn').text('Save Employee');
            new bootstrap.Modal('#employeeModal').show();
        } else if (type === 'edit') {
            uiService.clearForm('employee-form');
            $('#employeeModalTitle').text('Edit Employee');
            $('#save-employee-btn').text('Update Employee');
            uiService.populateForm(data);
            new bootstrap.Modal('#employeeModal').show();
        } else if (type === 'view') {
            const firstName = data.firstName || data.FirstName;
            const lastName = data.lastName || data.LastName;
            const department = data.department || data.Department;
            
            $('#view-avatar').text(uiService.getInitials(firstName, lastName));
            $('#view-name').text(`${firstName} ${lastName}`);
            
            const badgeClass = `bg-${uiService.getDeptColor(department)}`;
            $('#view-dept-badge').removeClass().addClass(`badge ${badgeClass} mt-2 mb-4`).text(department);
            
            $('#view-email').text(data.email || data.Email);
            $('#view-phone').text(data.phone || data.Phone);
            $('#view-designation').text(data.designation || data.Designation);
            $('#view-salary').text(uiService.formatCurrency(data.salary !== undefined ? data.salary : data.Salary));
            
            let dateVal = data.joinDate || data.JoinDate || '';
            if (dateVal && dateVal.includes('T')) dateVal = dateVal.split('T')[0];
            $('#view-joinDate').text(dateVal);
            
            const status = data.status || data.Status;
            const statusClass = status === 'Active' ? 'success' : 'danger';
            $('#view-status').html(`<span class="badge bg-${statusClass}">${status}</span>`);
            
            new bootstrap.Modal('#viewModal').show();
        } else if (type === 'delete') {
            const firstName = data.firstName || data.FirstName;
            const lastName = data.lastName || data.LastName;
            $('#delete-emp-name').text(`${firstName} ${lastName}`);
            $('#confirm-delete-btn').data('id', data.id || data.Id); 
            new bootstrap.Modal('#deleteModal').show();
        }
    },

    closeModal: (modalId) => {
        const modal = bootstrap.Modal.getInstance(document.getElementById(modalId));
        if (modal) modal.hide();
    },

    showToast: (message, type = 'success') => {
        const toastEl = $('#liveToast');
        toastEl.removeClass('bg-success bg-danger bg-info').addClass(`bg-${type}`);
        $('#toast-message').text(message);
        new bootstrap.Toast(toastEl[0]).show();
    }
};

if (typeof module !== 'undefined') module.exports = uiService;