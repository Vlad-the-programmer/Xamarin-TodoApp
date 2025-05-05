create database TodosDB;
use TodosDB;

-- Create User table
CREATE TABLE [User] (
    Id INT PRIMARY KEY IDENTITY(1,1),
    username VARCHAR(50) NOT NULL,
    password VARCHAR(50) NOT NULL,
    email VARCHAR(50),
    created_at DATETIME,
    RefreshToken NVARCHAR(MAX),
    RefreshTokenExpiry DATETIME
);

-- Create Tag table
CREATE TABLE Tag (
    Id INT PRIMARY KEY IDENTITY(1,1),
    name VARCHAR(50) NOT NULL UNIQUE -- UQ__Tags__72E12F1B...
);

-- Create Todo table
CREATE TABLE Todo (
    Id INT PRIMARY KEY IDENTITY(1,1),
    content NVARCHAR(500),
    user_id INT,
    title NVARCHAR(100),
    IsDone BIT NOT NULL DEFAULT 0,
    created_at DATETIME,
    updated_at DATETIME,
    CONSTRAINT FK_Todo_User FOREIGN KEY (user_id) REFERENCES [User](Id)
);

-- Index on Todo.user_id
CREATE INDEX IX_Notes_user_id ON Todo(user_id);

-- Create TodoTag table (junction table)
CREATE TABLE TodoTag (
    TodoId INT NOT NULL,
    TagId INT NOT NULL,
    Id INT NOT NULL IDENTITY(1,1), -- Not used as PK but may be helpful
    CONSTRAINT PK_TodoTag PRIMARY KEY (TodoId, TagId),
    CONSTRAINT FK_TodoTag_Todo FOREIGN KEY (TodoId) REFERENCES Todo(Id) ON DELETE CASCADE,
    CONSTRAINT FK_TodoTag_Tag FOREIGN KEY (TagId) REFERENCES Tag(Id) ON DELETE CASCADE
);

-- Index on TodoTag.TagId
CREATE INDEX IX_NoteTags_TagId ON TodoTag(TagId);

