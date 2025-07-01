# 🚗 RideSharingSystem

### What is RideSharingSystem?

**RideSharingSystem** is a lightweight, console-based ride-hailing simulation built in C#. It allows users to register as **Passengers** or **Drivers**, manage rides, process virtual payments, and track performance — all with data persisted locally through text files.

This project simulates the backend logic of popular ride-hailing platforms like Uber or Bolt but simplifies it into a command-line interface for educational and development practice purposes.

---

## 🎯 Why Choose RideSharingSystem?

- ✅ **Text File-Based Persistence** – No need for databases; ideal for local testing or academic demos.
- ✅ **LINQ-Powered Filtering** – Uses LINQ to simulate real-time driver filtering based on locations.
- ✅ **Wallet & Earnings Tracking** – Virtual wallets and real-time earnings per driver.
- ✅ **Admin Controls** – Built-in admin functionality to flag low-rated drivers.
- ✅ **Expandable Design** – Easily extensible with interfaces and class-based architecture.
- ✅ **Exception Handling** – Gracefully manages invalid input and runtime issues.

---

## 📚 Overview

RideSharingSystem is written in **C# ** and runs entirely in the **console**. Users can register, login, request or accept rides, and manage payment and rating data. The system simulates core ride-sharing logic without external dependencies.

---

## 📦 Components & Functional Requirements

### 1. 🔐 Authentication & Authorization

- Users can **register** as either **Passenger** or **Driver**.
- Credentials are hashed and stored in `users.txt`.
- Role-based menus control user access to features.
- Password validation with security checks.

### 2. 🧍 User & Profile Management

- Drivers and Passengers have distinct roles and capabilities.
- Each user has a persistent profile including:
  - Name, Role, Email, Password (hashed)
  - Wallet Balance (Passenger)
  - Earnings & Ratings (Driver)

### 3. 🚕 Ride Request & Assignment

- Passengers can request rides by selecting pickup and drop-off locations.
- Drivers **nearby** are filtered via LINQ.
- Rides have **statuses**: Pending → Accepted → Completed.
- Drivers can view and complete assigned rides.
- Ride data is persisted to `ride_history.txt`.

### 4. 💰 Wallet and Payment System

- Each ride calculates a **fare** based on simulated distance (e.g., string length).
- Passengers must have sufficient balance or the ride is denied.
- When accepted:
  - Fare is deducted from Passenger wallet.
  - Fare is added to Driver's earnings.
- All values persist between sessions.

### 5. 🌟 Ratings and Review System

- Passengers can rate drivers (1–5 stars) after completed rides.
- Driver ratings are averaged and stored.
- Admin can view drivers with low ratings (e.g., < 3).

### 6. 📊 Reports & Statistics

- Admin sees a summary of:
  - Total completed rides
  - Driver earnings
  - Average ratings
  - Flagged low-rated dri
