# PriceComparisonMVCAdmin

This is the admin panel for the [PriceComparisonWebAPI](https://github.com/Kostiantyn0101/PriceComparisonWebAPI) project. It is built using ASP.NET Core MVC and provides a management interface for moderators and administrators to control products, sellers, and categories within the price comparison ecosystem.

## 🔧 Technologies Used

* **ASP.NET Core MVC** (.NET 8)
* **Razor Views**
* **FluentValidation**
* **Bootstrap** for UI
* **HttpClient** for API communication
* **MemoryCache**
* **Token-based Authentication**

## 📁 Project Structure

* `Controllers/` - Admin controllers for managing products, sellers, and categories
* `Views/` - Razor pages organized by feature
* `Services/` - Logic for API communication and business rules
* `Filters/` - Custom filters (e.g. User info injection)
* `wwwroot/` - Static content (e.g. images, CSS, JS)

## 💼 Features

* Moderator dashboard for managing:

  * Base products and variants
  * Seller verification requests
  * Category structure and characteristics
* In-place editing of auction rates by category
* Modal windows with partial views for smoother UX
* TokenManager for seamless access token handling

## 🚀 Getting Started

### Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### Setup

```bash
# Clone the repository
https://github.com/Kostiantyn0101/PriceComparisonMVCAdmin.git

# Navigate to the project folder
cd PriceComparisonMVCAdmin

# Restore dependencies
dotnet restore

# Run the app
dotnet run
```

Make sure the [WebAPI backend](https://github.com/Kostiantyn0101/PriceComparisonWebAPI) is also running.

## 🔐 Authentication

This admin panel consumes a protected WebAPI using JWT. Ensure the `TokenManager` is properly initialized and tokens are acquired via login to use authorized endpoints.

---

## 🔗 Related repositories

* **Backend API** – [Kostiantyn0101/PriceComparisonWebAPI](https://github.com/Kostiantyn0101/PriceComparisonWebAPI)
* **Admin Panel** – [Kostiantyn0101/PriceComparison-UI-MVC-admin](https://github.com/Kostiantyn0101/PriceComparison-UI-MVC-admin)
* **PriceComparison MVC Front‑end** – [Kostiantyn0101/PriceComparison-UI-MVC](https://github.com/Kostiantyn0101/PriceComparison-UI-MVC)

## 🤝 Contributing

All improvements are welcome! Please fork the repo and open a PR.

---

## 📝 License

MIT – see [LICENSE](LICENSE) for details.