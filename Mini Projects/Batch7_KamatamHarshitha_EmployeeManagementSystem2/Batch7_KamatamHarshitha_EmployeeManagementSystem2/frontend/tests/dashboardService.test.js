global.API_BASE_URL = 'http://localhost:5000/api';

// Mock the authService dependency
global.authService = {
    getToken: jest.fn()
};

const dashboardService = require('../js/dashboardService');

describe('dashboardService', () => {
    beforeEach(() => {
        global.fetch = jest.fn();
        global.authService.getToken.mockReset();
    });

    test('getSummary() fetches data and includes Authorization header', async () => {
        // Arrange
        const mockSummary = { 
            total: 15, 
            active: 10, 
            inactive: 5, 
            departments: 4 
        };
        
        global.authService.getToken.mockReturnValue("test-token-123");
        
        global.fetch.mockResolvedValueOnce({
            ok: true,
            json: async () => mockSummary
        });

        // Act
        const result = await dashboardService.getSummary();

        // Assert
        expect(result).toEqual(mockSummary);
        expect(fetch).toHaveBeenCalledTimes(1);
        
        // Verify the Auth header was attached correctly
        const fetchOptions = fetch.mock.calls[0][1];
        expect(fetchOptions.headers['Authorization']).toBe('Bearer test-token-123');
    });

    test('getSummary() returns null on failed request', async () => {
        // Arrange
        global.fetch.mockResolvedValueOnce({
            ok: false
        });

        // Act
        const result = await dashboardService.getSummary();

        // Assert
        expect(result).toBeNull();
    });
});