const authService = (() => {
    // Private variable to hold session state in-memory. Lost on page refresh.
    let _session = null; 

    return {
        signup: async (username, password, role = 'Viewer') => {
            try {
                const response = await fetch(`${API_BASE_URL}/auth/register`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ username, password, role })
                });
                
                const data = await response.json();
                
                if (!response.ok) {
                    return { success: false, message: data.message || data.Email || "Registration failed." };
                }
                return { success: true };
            } catch (error) {
                console.error("Signup error:", error);
                return { success: false, message: "Network error occurred." };
            }
        },
        
        login: async (username, password) => {
            try {
                const response = await fetch(`${API_BASE_URL}/auth/login`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ username, password })
                });
                
                const data = await response.json();
                
                if (response.ok && data.success) {
                    // Store JWT and user details in-memory securely
                    _session = {
                        token: data.token,
                        username: data.username,
                        role: data.role
                    };
                    return true;
                }
                return false;
            } catch (error) {
                console.error("Login error:", error);
                return false;
            }
        },
        
        logout: () => {
            _session = null;
        },
        
        isLoggedIn: () => _session !== null,
        getCurrentUser: () => _session ? _session.username : null,
        getRole: () => _session ? _session.role : null,
        isAdmin: () => _session ? _session.role === 'Admin' : false,
        getToken: () => _session ? _session.token : null
    };
})();

if (typeof module !== 'undefined') module.exports = authService;