// Mock global variables
global.API_BASE_URL = 'http://localhost:5000/api';

const authService = require('../js/authService'); // Adjust path if necessary

describe('authService', () => {
    beforeEach(() => {
        // Reset fetch mock and clear any existing session
        global.fetch = jest.fn();
        authService.logout();
    });

    afterEach(() => {
        jest.restoreAllMocks();
    });

    test('signup() returns success: true on valid registration', async () => {
        // Arrange
        global.fetch.mockResolvedValueOnce({
            ok: true,
            json: async () => ({ success: true })
        });

        // Act
        const result = await authService.signup('newuser', 'password123', 'Viewer');

        // Assert
        expect(result.success).toBe(true);
        expect(fetch).toHaveBeenCalledWith(expect.stringContaining('/auth/register'), expect.any(Object));
    });

    test('signup() returns error message on conflict', async () => {
        // Arrange
        global.fetch.mockResolvedValueOnce({
            ok: false,
            json: async () => ({ message: "Username already exists." })
        });

        // Act
        const result = await authService.signup('admin', 'password123');

        // Assert
        expect(result.success).toBe(false);
        expect(result.message).toBe("Username already exists.");
    });

    test('login() stores token and returns true on valid credentials', async () => {
        // Arrange
        global.fetch.mockResolvedValueOnce({
            ok: true,
            json: async () => ({ 
                success: true, 
                token: "fake-jwt-token", 
                username: "admin", 
                role: "Admin" 
            })
        });

        // Act
        const result = await authService.login('admin', 'password123');

        // Assert
        expect(result).toBe(true);
        expect(authService.isLoggedIn()).toBe(true);
        expect(authService.getToken()).toBe("fake-jwt-token");
        expect(authService.isAdmin()).toBe(true);
    });

    test('login() returns false on invalid credentials', async () => {
        // Arrange
        global.fetch.mockResolvedValueOnce({
            ok: false,
            json: async () => ({ success: false, message: "Invalid credentials." })
        });

        // Act
        const result = await authService.login('admin', 'wrongpass');

        // Assert
        expect(result).toBe(false);
        expect(authService.isLoggedIn()).toBe(false);
        expect(authService.getToken()).toBeNull();
    });

    test('logout() clears the session', async () => {
         // Arrange (force a login first)
         global.fetch.mockResolvedValueOnce({
            ok: true,
            json: async () => ({ success: true, token: "token", username: "user", role: "Viewer" })
        });
        await authService.login('user', 'pass');
        expect(authService.isLoggedIn()).toBe(true);

        // Act
        authService.logout();

        // Assert
        expect(authService.isLoggedIn()).toBe(false);
        expect(authService.getToken()).toBeNull();
    });
});