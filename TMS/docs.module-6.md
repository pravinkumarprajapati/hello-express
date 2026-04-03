# Module 6 Delivery Notes

## Delivered
- Added SQL Server persistence foundation with EF Core `TmsDbContext`.
- Added SQL repository implementations for trainers, schedules, and notifications.
- Added infrastructure DI switch (`UseInMemoryRepositories`) to choose in-memory or SQL repositories.
- Added API `appsettings.json` with LocalDB connection string and toggle config.

## Next Module Preview
- Razor Pages calendar and dashboard UI.
- Azure Entra SSO integration with role-based authorization.
- Application Insights custom telemetry events.
