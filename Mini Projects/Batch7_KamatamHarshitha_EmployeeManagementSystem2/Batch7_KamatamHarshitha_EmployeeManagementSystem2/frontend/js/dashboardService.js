const dashboardService = {
    getSummary: async () => {
        try {
            const token = authService.getToken();
            const response = await fetch(`${API_BASE_URL}/employees/dashboard`, {
                method: 'GET',
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${token}`
                }
            });
            
            if (!response.ok) throw new Error('Failed to fetch dashboard data');
            return await response.json();
        } catch (error) {
            console.error("Dashboard fetch error:", error);
            return null;
        }
    }
};

if (typeof module !== 'undefined') module.exports = dashboardService;