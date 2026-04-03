const appData = {
    admin: { username: 'admin', password: 'admin@123' },
    employees: [
        { id: 1, firstName: 'Harshitha', lastName: 'Kamatam', email: 'harshitha.kamatam@gmail.com', phone: '9876543289', department: 'Engineering', designation: 'Software Engineer', salary: 950000, joinDate: '2021-03-15', status: 'Active' },
        { id: 2, firstName: 'Santhosh', lastName: 'Kamatam', email: 'santhosh.kamatam@yahoo.com', phone: '9123456790', department: 'Marketing', designation: 'Marketing Executive', salary: 680000, joinDate: '2020-07-01', status: 'Active' },
        { id: 3, firstName: 'Goutham', lastName: 'Kamatam', email: 'goutham.kamatam@outlook.com', phone: '9876512398', department: 'HR', designation: 'HR Executive', salary: 620000, joinDate: '2019-11-20', status: 'Active' },
        { id: 4, firstName: 'Ranaveer', lastName: 'Thota', email: 'ranaveer.thota@gmail.com', phone: '9988776601', department: 'Finance', designation: 'Financial Analyst', salary: 780000, joinDate: '2022-01-10', status: 'Active' },
        { id: 5, firstName: 'Thaswin', lastName: 'Miriyala', email: 'thaswin.miriyala@company.in', phone: '9123123187', department: 'Operations', designation: 'Operations Manager', salary: 1050000, joinDate: '2018-05-05', status: 'Active' },
        { id: 6, firstName: 'Rishitha', lastName: 'Kola', email: 'rishitha.kola@gmail.com', phone: '9988998845', department: 'Engineering', designation: 'Senior Developer', salary: 1350000, joinDate: '2017-09-12', status: 'Active' },
        { id: 7, firstName: 'Parthi', lastName: 'Miriyala', email: 'parthi.miriyala@yahoo.com', phone: '9001002099', department: 'Marketing', designation: 'Content Strategist', salary: 640000, joinDate: '2023-02-28', status: 'Inactive' },
        { id: 8, firstName: 'Sandeep', lastName: 'Kaliseety', email: 'sandeep.kaliseety@outlook.com', phone: '9112233492', department: 'Finance', designation: 'Accounts Manager', salary: 920000, joinDate: '2020-04-17', status: 'Active' },
        { id: 9, firstName: 'Chinni', lastName: 'Thota', email: 'chinni.thota@gmail.com', phone: '9998887721', department: 'Engineering', designation: 'DevOps Engineer', salary: 1080000, joinDate: '2021-08-22', status: 'Active' },
        { id: 10, firstName: 'Verri', lastName: 'Korsipati', email: 'verri.korsipati@company.in', phone: '9887766598', department: 'Operations', designation: 'Supply Chain Analyst', salary: 720000, joinDate: '2022-11-15', status: 'Active' },
        { id: 11, firstName: 'Yaswanth', lastName: 'Thota', email: 'yaswanth.thota@gmail.com', phone: '9776655491', department: 'Marketing', designation: 'Brand Manager', salary: 880000, joinDate: '2019-03-10', status: 'Active' },
        { id: 12, firstName: 'Krishna', lastName: 'Parlapalli', email: 'krishna.parlapalli@yahoo.com', phone: '9665544387', department: 'Finance', designation: 'Tax Consultant', salary: 810000, joinDate: '2021-06-05', status: 'Inactive' },
        { id: 13, firstName: 'Jaydav', lastName: 'Garre', email: 'jaydav.garre@outlook.com', phone: '9554433276', department: 'Engineering', designation: 'QA Engineer', salary: 760000, joinDate: '2022-09-01', status: 'Active' },
        { id: 14, firstName: 'Lucky', lastName: 'kamatam', email: 'lucky.kamatam@gmail.com', phone: '9443322165', department: 'HR', designation: 'Recruiter', salary: 580000, joinDate: '2023-01-20', status: 'Active' },
        { id: 15, firstName: 'Hanu', lastName: 'Mara', email: 'hanu.mara@company.in', phone: '9332211054', department: 'Operations', designation: 'Logistics Coordinator', salary: 620000, joinDate: '2020-10-12', status: 'Inactive' }
    ]
};

if (typeof module !== 'undefined') module.exports = appData;