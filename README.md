# Hotel Management System

A comprehensive hotel management application built with C# and Avalonia UI, developed as part of the **DIA (Development and Integration of Applications)** course at the University of Vigo (UVigo).

## 🎯 Project Overview

This project represents my first exploration into C# and the Avalonia framework, as well as my second experience with graphical user interfaces and UI development. The application provides a complete hotel management solution with desktop UI capabilities.

## ✨ Features

### Core Functionality
- **Room Management**: Add, edit, and manage hotel rooms with amenities
- **Client Management**: Complete customer registration and information system
- **Reservation System**: Book and track room reservations with date management
- **Advanced Search**: Find rooms, clients, and reservations with multiple filter options
- **Statistics & Analytics**: Visual charts showing occupancy rates and usage statistics

### Data Management
- **XML Persistence**: All data stored in XML format for easy portability
- **Real-time Updates**: Changes are immediately reflected across the application
- **Data Validation**: Form validation ensures data integrity

### Visual Analytics
The application includes several chart types:
- General occupancy rates
- Room-specific occupancy statistics
- Client occupancy patterns
- Amenity usage analytics

## 🛠️ Technical Stack

- **Framework**: .NET 9.0
- **UI Framework**: Avalonia UI 11.3.6
- **Language**: C# 
- **Data Storage**: XML serialization
- **Architecture**: MVVM-inspired pattern with Core/View separation

## 📁 Project Structure

```
Hotel/
├── Core/                          # Business logic and data models
│   ├── Cliente.cs                 # Client entity
│   ├── Habitacion.cs              # Room entity
│   ├── Reserva.cs                 # Reservation entity
│   ├── Comodidad.cs               # Amenity entity
│   ├── HotelManager.cs            # Data management and XML operations
│   └── Graficos/                  # Chart generation classes
│       ├── GraficoBase.cs         # Base chart class
│       ├── GraficoOcupacionGeneral.cs
│       ├── GraficoOcupacionHabitacion.cs
│       ├── GraficoOcupacionCliente.cs
│       └── GraficoComodidades.cs
├── View/                         # User interface components
│   ├── MainWindow.axaml          # Main application window
│   ├── AddRoomWindow.axaml       # Room management interface
│   ├── AddClientWindow.axaml     # Client management interface
│   ├── AddBookingWindow.axaml    # Reservation interface
│   ├── SearchWindow.axaml        # Search functionality
│   └── GraphWindow.axaml         # Statistics and charts
├── *.xml                         # Data storage files
└── Hotel.csproj                  # Project configuration
```

## 🚀 Getting Started

### Prerequisites
- .NET 9.0 SDK or later
- Visual Studio 2022, JetBrains Rider, or VS Code with C# extension

### Installation & Setup

1. **Clone the repository**:
   ```bash
   git clone [repository-url]
   cd Hotel
   ```

2. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

3. **Build the project**:
   ```bash
   dotnet build
   ```

4. **Run the application**:
   ```bash
   dotnet run
   ```

## 💡 How to Use

### First Launch
Upon first startup, the application automatically creates default amenities and initializes the data storage system.

### Navigation
The main window provides easy navigation through six main sections:
- **Clientes** (Clients): Manage customer information
- **Habitaciones** (Rooms): Add and configure hotel rooms
- **Reservas** (Reservations): Create and manage bookings
- **Búsquedas** (Search): Find specific records
- **Estadísticas** (Statistics): View occupancy and usage charts
- **Salir** (Exit): Close the application

### Data Persistence
All data is automatically saved to XML files:
- `clientes.xml` - Customer information
- `habitaciones.xml` - Room configurations
- `reservas.xml` - Booking records
- `comodidades.xml` - Available amenities

## 📊 Features in Detail

### Room Management
- Configure room numbers, types, and capacity
- Assign multiple amenities per room
- Set pricing and availability

### Reservation System
- Date-based booking with conflict detection
- Link reservations to existing clients and rooms
- Track booking status and duration

### Analytics Dashboard
Generate visual reports including:
- Hotel-wide occupancy trends
- Individual room performance
- Client booking patterns
- Most popular amenities

## 🎓 Academic Context

This project was developed for the **DIA (Development and Integration of Applications)** course in the 4th year of Computer Engineering at the University of Vigo. It represents:

- **First C# Project**: Introduction to C# language and .NET ecosystem
- **First Avalonia Experience**: Learning cross-platform UI development
- **Second GUI Project**: Building on previous UI/UX experience
- **Academic Milestone**: Demonstrating software engineering principles

## 🔧 Architecture Highlights

### Separation of Concerns
- **Core**: Business logic, data models, and persistence
- **View**: User interface and presentation logic
- **Data Layer**: XML-based storage with serialization

### Design Patterns
- Repository pattern in `HotelManager`
- Strategy pattern for different chart types
- Observer pattern for UI updates

## 🤝 Contributing

This is an academic project, but suggestions and improvements are welcome. Feel free to:
- Report bugs or issues
- Suggest feature enhancements
- Provide code improvements

## 📄 License

This project is developed for educational purposes as part of university coursework.

## 👨‍💻 Author

Developed by Rubén Bouzón as part of the Computer Engineering program at the University of Vigo (UVigo).

---

⭐ *This project showcases the practical application of software development principles learned in the DIA course, demonstrating proficiency in C#, Avalonia UI framework, and desktop application development.*