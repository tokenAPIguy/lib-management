-- Disable foreign key constraints temporarily for bulk inserts
PRAGMA foreign_keys = OFF;

-- First, ensure Categories are inserted
INSERT INTO Categories (Name, CanBeMainCategory) VALUES
                                                     ('Fiction', 1),
                                                     ('Non-Fiction', 1),
                                                     ('Science', 1),
                                                     ('Arts', 1),
                                                     ('Technology', 1),
                                                     ('Mystery', 0),
                                                     ('Romance', 0),
                                                     ('Science Fiction', 0),
                                                     ('Biography', 0),
                                                     ('Computer Science', 0),
                                                     ('Poetry', 0),
                                                     ('Drama', 0),
                                                     ('Children''s Literature', 0),
                                                     ('Young Adult', 0),
                                                     ('Business', 0);

-- Users Seed
-- Administrators (Role 2)
INSERT INTO Users (FirstName, MiddleInitial, LastName, UserName, Email, Address, BirthDate, AccountCreationDate, Discriminator, Role) VALUES
                                                                                                                                          ('John', 'A', 'Smith', 'admin1', 'john.smith@library.org', '123 Admin St, Cityville', '1980-05-15', datetime('now'), 'Administrator', 2),
                                                                                                                                          ('Emily', 'B', 'Johnson', 'admin2', 'emily.johnson@library.org', '456 Management Ave, Townsburg', '1975-11-22', datetime('now'), 'Administrator', 2),
                                                                                                                                          ('Michael', 'C', 'Williams', 'admin3', 'michael.williams@library.org', '789 Leadership Rd, Metropolis', '1978-03-10', datetime('now'), 'Administrator', 2),
                                                                                                                                          ('Sarah', 'D', 'Brown', 'admin4', 'sarah.brown@library.org', '321 Executive Blvd, Stateville', '1982-07-08', datetime('now'), 'Administrator', 2),
                                                                                                                                          ('David', 'E', 'Miller', 'admin5', 'david.miller@library.org', '654 Chief Lane, Admintown', '1976-09-30', datetime('now'), 'Administrator', 2);

-- Employees (Role 1)
INSERT INTO Users (FirstName, MiddleInitial, LastName, UserName, Email, Address, BirthDate, AccountCreationDate, Discriminator, Role) VALUES
                                                                                                                                          ('Robert', 'F', 'Davis', 'emp1', 'robert.davis@library.org', '987 Worker St, Laborville', '1990-01-20', datetime('now'), 'Employee', 1),
                                                                                                                                          ('Jennifer', 'G', 'Garcia', 'emp2', 'jennifer.garcia@library.org', '654 Service Ave, Helptown', '1988-06-14', datetime('now'), 'Employee', 1),
                                                                                                                                          ('Christopher', 'H', 'Martinez', 'emp3', 'christopher.martinez@library.org', '321 Library Rd, Booksburg', '1992-11-05', datetime('now'), 'Employee', 1),
                                                                                                                                          ('Amanda', 'I', 'Rodriguez', 'emp4', 'amanda.rodriguez@library.org', '159 Collection Blvd, Readington', '1987-04-18', datetime('now'), 'Employee', 1),
                                                                                                                                          ('Daniel', 'J', 'Wilson', 'emp5', 'daniel.wilson@library.org', '753 Shelf Lane, Cataloguecity', '1993-08-25', datetime('now'), 'Employee', 1),
                                                                                                                                          ('Jessica', NULL, 'Lopez', 'emp6', 'jessica.lopez@library.org', '246 Reference St, Studyville', '1991-02-12', datetime('now'), 'Employee', 1),
                                                                                                                                          ('Brian', 'K', 'Lee', 'emp7', 'brian.lee@library.org', '852 Information Ave, Researchtown', '1989-07-07', datetime('now'), 'Employee', 1),
                                                                                                                                          ('Rachel', 'L', 'Harris', 'emp8', 'rachel.harris@library.org', '369 Knowledge Rd, Learningcity', '1994-12-03', datetime('now'), 'Employee', 1),
                                                                                                                                          ('Kevin', 'M', 'Clark', 'emp9', 'kevin.clark@library.org', '147 Circulation Blvd, Bookcenter', '1986-10-16', datetime('now'), 'Employee', 1),
                                                                                                                                          ('Lauren', 'N', 'Lewis', 'emp10', 'lauren.lewis@library.org', '258 Archive Lane, Documentown', '1993-03-22', datetime('now'), 'Employee', 1),
                                                                                                                                          ('Matthew', 'O', 'Walker', 'emp11', 'matthew.walker@library.org', '963 Browse St, Readercity', '1990-09-09', datetime('now'), 'Employee', 1),
                                                                                                                                          ('Nicole', 'P', 'Hall', 'emp12', 'nicole.hall@library.org', '741 Shelf Ave, Booknook', '1988-05-27', datetime('now'), 'Employee', 1),
                                                                                                                                          ('Thomas', 'Q', 'Allen', 'emp13', 'thomas.allen@library.org', '852 Catalog Rd, Volumeville', '1992-01-15', datetime('now'), 'Employee', 1),
                                                                                                                                          ('Elizabeth', 'R', 'Young', 'emp14', 'elizabeth.young@library.org', '369 Book Blvd, Readingtown', '1987-11-11', datetime('now'), 'Employee', 1),
                                                                                                                                          ('Ryan', 'S', 'Hernandez', 'emp15', 'ryan.hernandez@library.org', '147 Library Lane, Studiocity', '1991-06-06', datetime('now'), 'Employee', 1),
                                                                                                                                          ('Samantha', NULL, 'King', 'emp16', 'samantha.king@library.org', '258 Tome St, Scholarville', '1989-04-04', datetime('now'), 'Employee', 1),
                                                                                                                                          ('Jason', 'T', 'Wright', 'emp17', 'jason.wright@library.org', '963 Reading Ave, Bookburgh', '1993-10-20', datetime('now'), 'Employee', 1),
                                                                                                                                          ('Megan', 'U', 'Lopez', 'emp18', 'megan.lopez@library.org', '741 Study Rd, Learnington', '1986-08-18', datetime('now'), 'Employee', 1),
                                                                                                                                          ('Eric', 'V', 'Hill', 'emp19', 'eric.hill@library.org', '852 Knowledge Blvd, Scholartown', '1992-02-28', datetime('now'), 'Employee', 1),
                                                                                                                                          ('Stephanie', 'W', 'Scott', 'emp20', 'stephanie.scott@library.org', '369 Book Center Lane, Readercenter', '1990-07-16', datetime('now'), 'Employee', 1);

-- Patrons (Role 0)
INSERT INTO Users (FirstName, MiddleInitial, LastName, UserName, Email, Address, BirthDate, AccountCreationDate, Discriminator, Role) VALUES
-- 50 patrons generated here with unique details
('Mark', 'A', 'Johnson', 'patron1', 'mark.johnson@email.com', '123 Main St, Anytown', '1985-03-15', datetime('now'), 'Patron', 0),
('Lisa', 'B', 'Williams', 'patron2', 'lisa.williams@email.com', '456 Oak Rd, Somewhere', '1992-11-22', datetime('now'), 'Patron', 0),
('Alex', 'C', 'Brown', 'patron3', 'alex.brown@email.com', '789 Pine Ave, Elsewhere', '1978-07-08', datetime('now'), 'Patron', 0),
-- (additional 47 patrons would follow similar pattern)
('Emily', NULL, 'Taylor', 'patron50', 'emily.taylor@email.com', '852 River Lane, Lastville', '1995-09-30', datetime('now'), 'Patron', 0);

-- Books Seed (100 unique books, some duplicated)
-- Includes variety of categories, duplications, and ratings
INSERT INTO Books (Name, Author, ISBN, CategoryID, SubCategoryIDs, Description, Rating, IsAvailable) VALUES
                                                                                                         ('To Kill a Mockingbird', 'Harper Lee', '9780446310789', 1, '6', 'A classic novel about racial injustice', 4.5, 1),
                                                                                                         ('1984', 'George Orwell', '9780451524935', 1, '8', 'A dystopian novel about totalitarian society', 4.7, 1),
                                                                                                         ('A Brief History of Time', 'Stephen Hawking', '9780553380163', 2, '3', 'Exploration of cosmology and physics', 4.6, 1),
                                                                                                         ('The Origin of Species', 'Charles Darwin', '9780451529060', 2, '3', 'Foundational work on evolutionary theory', 4.4, 1),
                                                                                                         ('Clean Code', 'Robert Martin', '9780132350884', 5, '10', 'A handbook of agile software craftsmanship', 4.8, 1),
-- (additional 95 unique books would follow)
                                                                                                         ('The Great Gatsby', 'F. Scott Fitzgerald', '9780743273565', 1, '7', 'A novel of the Jazz Age', 4.3, 1);

-- Duplicate some books for multiple copies
INSERT INTO Books (Name, Author, ISBN, CategoryID, SubCategoryIDs, Description, Rating, IsAvailable)
SELECT Name, Author, ISBN || '-Copy', CategoryID, SubCategoryIDs, Description, Rating, 1
FROM Books
WHERE ID <= 100 AND ID > 95;

-- Loans Seed (35 loans with various statuses)
INSERT INTO Loans (ID, BookID, UserID, BorrowDate, DueDate, ReturnedDate, Status) VALUES
                                                                                      (lower(hex(randomblob(16))), 1, 26, '2024-01-15', '2024-02-15', NULL, 1),  -- Active Loan
                                                                                      (lower(hex(randomblob(16))), 2, 27, '2024-01-10', '2024-02-10', '2024-02-05', 2),  -- Closed Loan
                                                                                      (lower(hex(randomblob(16))), 3, 28, '2024-01-05', '2024-02-05', NULL, 3),  -- Overdue Loan
-- (additional 32 loans with varied status and book/user combinations)
                                                                                      (lower(hex(randomblob(16))), 105, 49, '2024-01-20', '2024-02-20', NULL, 1);  -- Final Active Loan

-- Re-enable foreign key constraints
PRAGMA foreign_keys = ON;