-- On supprime l'ancienne base pour être sûr de repartir à zéro
DROP DATABASE IF EXISTS bama;

-- Create the database if it does not exist
CREATE DATABASE IF NOT EXISTS bama;
USE bama;

-- Create Customer table if it does not exist
CREATE TABLE IF NOT EXISTS Customer (
    CustomerId INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(191) NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL,
    FirstName VARCHAR(100) NULL,
    LastName VARCHAR(100) NULL,
    Email VARCHAR(100) NULL,
    PhoneNumber VARCHAR(20) NULL
);


-- Create Burger table if it does not exist
CREATE TABLE IF NOT EXISTS Burger (
    BurgerId INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(191) NOT NULL UNIQUE,
    Description TEXT,
    Price DECIMAL(10, 2) NOT NULL,
    Stock INT UNSIGNED DEFAULT 0,
    ImageUrl VARCHAR(2048)
);


-- Create Order table if it does not exist
CREATE TABLE IF NOT EXISTS CustomerOrder (
    CustomerOrderId INT AUTO_INCREMENT PRIMARY KEY,
    CustomerId INT NOT NULL,
    Status VARCHAR(50) DEFAULT 'InProgress',
    FOREIGN KEY (CustomerId) REFERENCES Customer(CustomerId) ON DELETE CASCADE
);


-- Create OrderItem table if it does not exist
CREATE TABLE IF NOT EXISTS OrderItem (
    OrderItemId INT AUTO_INCREMENT PRIMARY KEY,
    CustomerOrderId INT NOT NULL,
    BurgerId INT NOT NULL,
    Quantity INT UNSIGNED NOT NULL DEFAULT 1,
    Price DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (CustomerOrderId) REFERENCES CustomerOrder(CustomerOrderId) ON DELETE CASCADE,
    FOREIGN KEY (BurgerId) REFERENCES Burger(BurgerId) ON DELETE CASCADE,
    UNIQUE (CustomerOrderId, BurgerId)
);



INSERT INTO Burger (Name, Description, Price, Stock, ImageUrl)
VALUES
('Cheeseburger', 'Burger avec cheddar et steak', 6.99, 50, 'assets/images/hamburgers.jpg'),
('Bacon Burger', 'Burger avec bacon croustillant', 7.49, 40, 'assets/images/hamburgers.jpg'),
('Veggie Burger', 'Burger végétarien au tofu', 5.99, 30, 'assets/images/hamburgers.jpg'),
('Double Beef Burger', 'Double steak, double fromage', 8.99, 25, 'assets/images/hamburgers.jpg'),
('Chicken Burger', 'Burger au poulet grillé', 6.49, 35, 'assets/images/hamburgers.jpg');