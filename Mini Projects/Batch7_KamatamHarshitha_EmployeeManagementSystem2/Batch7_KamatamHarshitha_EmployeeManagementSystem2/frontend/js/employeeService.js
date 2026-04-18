const employeeService = {
    // We now construct a query string and pass it to storageService
    getAll: async (params) => {
        const query = new URLSearchParams();
        if (params.search) query.append('Search', params.search);
        if (params.department) query.append('Department', params.department);
        if (params.status) query.append('Status', params.status);
        if (params.sortBy) query.append('SortBy', params.sortBy);
        if (params.sortDir) query.append('SortDir', params.sortDir);
        query.append('Page', params.page || 1);
        query.append('PageSize', params.pageSize || 10);

        return await storageService.getAll(`?${query.toString()}`);
    },
    
    getById: async (id) => await storageService.getById(id),
    add: async (data) => await storageService.add(data),
    update: async (id, data) => await storageService.update(id, data),
    remove: async (id) => await storageService.remove(id),
    
    // Departments are static as per the requirement constraints
    getUniqueDepartments: () => ['Engineering', 'Finance', 'HR', 'Marketing', 'Operations']
};

if (typeof module !== 'undefined') module.exports = employeeService;