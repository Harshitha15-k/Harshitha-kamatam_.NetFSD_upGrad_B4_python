const validationService = {
    // Client-side validation remains exactly the same to prevent unnecessary API calls
    validateEmployeeForm: (data) => {
        const errors = {};
        if (!data.firstName.trim()) errors.firstName = "First Name is required";
        if (!data.lastName.trim()) errors.lastName = "Last Name is required";
        
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!data.email.trim()) errors.email = "Email is required";
        else if (!emailRegex.test(data.email)) errors.email = "Invalid Email format";

        if (!data.phone.trim()) errors.phone = "Phone Number is required";
        else if (!/^\d{10}$/.test(data.phone)) errors.phone = "Must be a 10-digit number";

        if (!data.department) errors.department = "Select a Department";
        if (!data.designation.trim()) errors.designation = "Designation is required";
        
        if (!data.salary) errors.salary = "Salary is required";
        else if (Number(data.salary) <= 0) errors.salary = "Must be a positive number";

        if (!data.joinDate) errors.joinDate = "Join Date is required";
        if (!data.status) errors.status = "Select a Status";

        return Object.keys(errors).length > 0 ? errors : null;
    },
    
    validateAuthForm: (username, password, confirmPassword = null) => {
        const errors = {};
        if (!username.trim()) errors.username = "Username required";
        if (!password) errors.password = "Password required";
        else if (password.length < 6) errors.password = "Minimum 6 characters";
        
        if (confirmPassword !== null && password !== confirmPassword) {
            errors.confirm = "Passwords do not match";
        }
        return Object.keys(errors).length > 0 ? errors : null;
    },

    // NEW: Map .NET API Validation/Conflict errors to UI fields
    mapServerErrors: (serverData) => {
        const errors = {};
        if (!serverData) return errors;

        // If it's a 400 Bad Request from .NET Data Annotations (it returns an 'errors' object)
        if (serverData.errors) {
            Object.keys(serverData.errors).forEach(key => {
                // .NET typically capitalizes field names (e.g., 'FirstName'). 
                // We need to lower-case the first letter to match our HTML IDs ('firstName').
                const camelKey = key.charAt(0).toLowerCase() + key.slice(1);
                errors[camelKey] = serverData.errors[key][0]; 
            });
        } 
        // If it's a 409 Conflict from our custom controller logic (e.g., duplicate email)
        else if (serverData.Email) {
             errors.email = serverData.Email;
        }
        // Handle generic message
        else if (serverData.message) {
             errors.general = serverData.message;
        }

        return errors;
    }
};

if (typeof module !== 'undefined') module.exports = validationService;