Agent mode with Claude Opus 4.5

1. fix error "There is already an object named 'AspNetRoles' in the database." when running dotnet ef database update

2. add system wide audittrail  feature in this app
3. add Role based access with User management page to administer User and assign roles to each user.
4. system has 3 roles Admin, Manager and User. Admin has full access to all features, Manager has access to manage users and view audit trails, User has access to view their own data and audit trails.
	•	Default Admin User:
	Username: admin
	Email: admin@aims.local
	Password: Admin@123
5. add a feature to log  all user activities in the system, such as login, logout, data changes to audit trail. This will help in monitoring and auditing user actions for security and compliance purposes.
// Inject IActivityLogger
private readonly IActivityLogger _activityLogger;

// Log a security activity
await _activityLogger.LogSecurityActivityAsync(
    ActivityType.Login,
    "User logged in successfully",
    "Success",
    userId,
    userName);

// Log a general activity
await _activityLogger.LogActivityAsync(
    ActivityType.UserCreated,
    "Created new user 'john.doe'",
    "ApplicationUser",
    userId,
    "Success");
