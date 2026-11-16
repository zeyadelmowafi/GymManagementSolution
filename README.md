# Gym Management Solution

Three-layer ASP.NET Core MVC solution made from the supplied tutorial videos and presentation material.

## Projects

- `GymManagementSolution.PL`: MVC interface, controllers, dashboard, and startup seeding.
- `GymManagementSolution.BLL`: view models, AutoMapper profile, result type, and application services.
- `GymManagementSolution.DAL`: gym entities, EF Core context, generic repository, session repository, and Unit of Work.

## Included features

- Session list, details, create, edit, delete, status and available-slot calculation.
- Session validation for dates, capacity, trainer specialty, and trainer time conflicts.
- Trainer management with Egyptian mobile number and unique contact validation.
- Membership plans loaded from `plans.json` on the first start when the database has no plans.
- Member list and create form.
- Dashboard counts for all six gym statistics.
- GymUser inheritance, addresses, memberships, bookings, and health records.

## Run locally

Open `GymManagementSolution.sln` in Visual Studio 2022 with the .NET 9 SDK and ASP.NET workload. The connection string in `GymManagementSolution.PL/appsettings.json` uses SQL Server LocalDB. Ensure LocalDB is installed and running, then set `GymDatabase` to your SQL Server instance if needed. The app creates the database and initial categories automatically, then seeds plans from `plans.json` when no plans exist.

Restore NuGet packages and set `GymManagementSolution.PL` as the startup project before running.
