const storageService = (() => {
    // Helper function to build headers and attach the JWT token
    const _headers = (withAuth = true) => {
        const headers = { "Content-Type": "application/json" };
        if (withAuth) {
            const token = authService.getToken();
            if (token) headers["Authorization"] = `Bearer ${token}`;
        }
        return headers;
    };

    return {
        // The query string handles all pagination, filtering, and sorting on the server
        getAll: async (queryString = '') => {
            const response = await fetch(`${API_BASE_URL}/employees${queryString}`, {
                method: 'GET',
                headers: _headers()
            });
            if (!response.ok) throw new Error('Failed to fetch employees');
            return await response.json(); // Returns the PagedResult JSON envelope
        },

        getById: async (id) => {
            const response = await fetch(`${API_BASE_URL}/employees/${id}`, {
                method: 'GET',
                headers: _headers()
            });
            if (!response.ok) throw new Error('Failed to fetch employee');
            return await response.json();
        },

        add: async (employeeData) => {
            const response = await fetch(`${API_BASE_URL}/employees`, {
                method: 'POST',
                headers: _headers(),
                body: JSON.stringify(employeeData)
            });
            // If the API returns 409 Conflict (e.g. duplicate email), we throw the payload to show field errors
            if (!response.ok) {
                const errorData = await response.json();
                throw { status: response.status, data: errorData };
            }
            return await response.json();
        },

        update: async (id, employeeData) => {
            const response = await fetch(`${API_BASE_URL}/employees/${id}`, {
                method: 'PUT',
                headers: _headers(),
                body: JSON.stringify(employeeData)
            });
            if (!response.ok) {
                const errorData = await response.json();
                throw { status: response.status, data: errorData };
            }
            return await response.json();
        },

        remove: async (id) => {
            const response = await fetch(`${API_BASE_URL}/employees/${id}`, {
                method: 'DELETE',
                headers: _headers()
            });
            if (!response.ok) throw new Error('Failed to delete employee');
            return await response.json();
        }
    };
})();

if (typeof module !== 'undefined') module.exports = storageService;