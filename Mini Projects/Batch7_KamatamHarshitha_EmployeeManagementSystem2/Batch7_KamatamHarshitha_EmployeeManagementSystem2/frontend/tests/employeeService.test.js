// Mock the storageService dependency
global.storageService = {
    getAll: jest.fn(),
    getById: jest.fn(),
    add: jest.fn(),
    update: jest.fn(),
    remove: jest.fn()
};

const employeeService = require('../js/employeeService');

describe('employeeService', () => {
    beforeEach(() => {
        jest.clearAllMocks();
    });

    test('getAll() builds correct query string from params', async () => {
        // Arrange
        const mockParams = {
            search: "john",
            department: "IT",
            status: "Active",
            sortBy: "salary",
            sortDir: "desc",
            page: 2,
            pageSize: 15
        };

        global.storageService.getAll.mockResolvedValueOnce({ data: [], totalCount: 0 });

        // Act
        await employeeService.getAll(mockParams);

        // Assert
        expect(global.storageService.getAll).toHaveBeenCalledTimes(1);
        
        const passedQueryString = global.storageService.getAll.mock.calls[0][0];
        
        // Check that all params were appended correctly
        expect(passedQueryString).toContain('Search=john');
        expect(passedQueryString).toContain('Department=IT');
        expect(passedQueryString).toContain('Status=Active');
        expect(passedQueryString).toContain('SortBy=salary');
        expect(passedQueryString).toContain('SortDir=desc');
        expect(passedQueryString).toContain('Page=2');
        expect(passedQueryString).toContain('PageSize=15');
    });

    test('getAll() provides default pagination values', async () => {
        // Arrange
        global.storageService.getAll.mockResolvedValueOnce({ data: [], totalCount: 0 });

        // Act - Call with empty params
        await employeeService.getAll({});

        // Assert
        const passedQueryString = global.storageService.getAll.mock.calls[0][0];
        expect(passedQueryString).toContain('Page=1');
        expect(passedQueryString).toContain('PageSize=10');
    });

    test('getById() delegates to storageService', async () => {
        await employeeService.getById(5);
        expect(global.storageService.getById).toHaveBeenCalledWith(5);
    });

    test('add() delegates to storageService', async () => {
        const data = { firstName: "Test" };
        await employeeService.add(data);
        expect(global.storageService.add).toHaveBeenCalledWith(data);
    });

    test('update() delegates to storageService', async () => {
        const data = { firstName: "Update" };
        await employeeService.update(1, data);
        expect(global.storageService.update).toHaveBeenCalledWith(1, data);
    });

    test('remove() delegates to storageService', async () => {
        await employeeService.remove(3);
        expect(global.storageService.remove).toHaveBeenCalledWith(3);
    });

    test('getUniqueDepartments() returns static array', () => {
        const depts = employeeService.getUniqueDepartments();
        expect(depts).toContain('Engineering');
        expect(depts.length).toBe(5);
    });
});