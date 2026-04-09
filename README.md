# RentACar Management System - Backend API

## 🏛 Architecture: Onion Architecture
This project is implemented using **Onion Architecture** to ensure high maintainability and a clear separation of concerns. The architecture is designed to keep the core domain logic independent of external concerns like databases and UI frameworks.

### 📂 Project Structure & Layers
* **Domain Layer:** The core of the application containing entities (Car, Brand, Color, Customer, Rental, etc.) and domain-specific interfaces.
* **Application Layer:** Orchestrates business logic, handles DTO (Data Transfer Object) mapping, and defines service interfaces.
* **Persistence Layer:** Manages data access using **Entity Framework Core** and implements the Repository Pattern.
* **Infrastructure Layer:** Handles cross-cutting concerns and external system integrations.
* **WebAPI Layer:** The entry point of the system, exposing RESTful endpoints built with **ASP.NET Core Controllers**.

## 🛠 Technical Stack
* **Language & Framework:** C# / .NET 8.0
* **ORM:** Entity Framework Core (Code-First Approach)
* **Database:** Microsoft SQL Server
* **Design Patterns:** Repository Pattern, Dependency Injection, and Unit of Work

## 🚀 Development Status
* **Timeline:** The core development of this project was carried out between **July and August 2025**.
* **Current State:** The system is currently hosted and tested in a **local environment**; it has not been deployed to a live production server yet.
* **Future Work:** The project is under active refinement, with planned updates for further optimization and feature enhancements.

## 👥 Contributors
* **Ata Emir Karamuk** - Lead Backend Developer
