CREATE TABLE category (
	category_id INT IDENTITY (1,1) PRIMARY KEY,
	category_name NVARCHAR(255) UNIQUE NOT NULL,
	is_delete BIT DEFAULT 0
);

CREATE TABLE supplier (
	supplier_id INT IDENTITY (1,1) PRIMARY KEY,
	supplier_name NVARCHAR(255) UNIQUE NOT NULL,
	is_delete BIT DEFAULT 0
);

CREATE TABLE good (
	good_id INT IDENTITY (1,1) PRIMARY KEY,
	category_id INT NOT NULL,
	supplier_id INT NOT NULL,
	good_code NVARCHAR(10) UNIQUE NOT NULL,
	good_name NVARCHAR(255) NOT NULL,
	good_stock INT,
	CONSTRAINT fk_category FOREIGN KEY (category_id) REFERENCES category(category_id),
	CONSTRAINT fk_supplier FOREIGN KEY (supplier_id) REFERENCES supplier(supplier_id)
);

CREATE TABLE good_mutation (
	mutation_id INT IDENTITY (1,1) PRIMARY KEY,
	good_id INT NOT NULL,
    category_id INT NOT NULL,
    supplier_id INT NOT NULL,
	mutation_date DATETIME,
	status NVARCHAR(10) NOT NULL,
	amount INT NOT NULL,
	CONSTRAINT fk_good FOREIGN KEY (good_id) REFERENCES good(good_id),
    CONSTRAINT fk_category_good_mutation FOREIGN KEY (category_id) REFERENCES category(category_id),
	CONSTRAINT fk_supplier_good_mutation FOREIGN KEY (supplier_id) REFERENCES supplier(supplier_id)
);

CREATE INDEX idx_category_name ON category (category_name);
CREATE INDEX idx_supplier_name ON supplier (supplier_name);
CREATE INDEX idx_good_code ON good (good_code);
CREATE INDEX idx_good_name ON good (good_name);
CREATE INDEX idx_good_mutation_date ON good_mutation (mutation_date);

CREATE OR ALTER PROCEDURE GetCategory
    @Keyword NVARCHAR(100) = NULL,
    @CursorId INT = 0
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 10
        category_id AS CategoryId,
        category_name AS CategoryName
    FROM category
    WHERE category_id > @CursorId
      AND (
            @Keyword IS NULL
            OR @Keyword = ''
            OR LOWER(category_name) LIKE LOWER('%' + @Keyword + '%')
          )
      AND is_delete = 0
    ORDER BY category_id ASC;
END;


CREATE OR ALTER PROCEDURE GetSupplier
    @Keyword NVARCHAR(100) = NULL,
    @CursorId INT = 0
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 10
        supplier_id AS SupplierId,
        supplier_name AS SupplierName
    FROM supplier
    WHERE supplier_id > @CursorId
      AND (
            @Keyword IS NULL
            OR @Keyword = ''
            OR LOWER(supplier_name) LIKE LOWER('%' + @Keyword + '%')
          )
      AND is_delete = 0
    ORDER BY supplier_id ASC;
END;


CREATE OR ALTER PROCEDURE GetGood
    @Keyword NVARCHAR(100) = NULL,
    @CursorId INT = 0
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 10
            g.good_id AS goodId,
            c.category_name AS CategoryName,
            s.supplier_name AS SupplierName,
            g.good_code AS GoodCode,
            g.good_name AS GoodName,
            g.good_stock AS GoodStock
    FROM good g
    INNER JOIN category c ON c.category_id = g.category_id
    INNER JOIN supplier s ON s.supplier_id = g.supplier_id
    WHERE g.good_id > @CursorId
      AND (
            @Keyword IS NULL
            OR @Keyword = ''
            OR LOWER(g.good_name) LIKE LOWER('%' + @Keyword + '%')
          )
    ORDER BY g.good_id ASC;
END;


CREATE OR ALTER PROCEDURE GetMutation
    @Keyword NVARCHAR(100) = NULL,
    @CursorId INT = 0
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 10
        gm.mutation_id AS MutationId,
        gm.mutation_date AS MutationDate,
        g.good_id AS GoodId,
        g.good_name AS GoodName,
        c.category_id AS CategoryId,
        c.category_name AS CategoryName,
        s.supplier_id AS SupplierId,
        s.supplier_name AS SupplierName,
        gm.status,
        gm.amount
    FROM good_mutation gm
    INNER JOIN good g ON g.good_id = gm.good_id
    INNER JOIN category c ON c.category_id = gm.category_id
    INNER JOIN supplier s ON s.supplier_id = gm.supplier_id
    WHERE gm.mutation_id > @CursorId
      AND (
            @Keyword IS NULL
            OR @Keyword = ''
            OR CONVERT(DATE, gm.mutation_date) = TRY_CONVERT(DATE, @Keyword)
          )
    ORDER BY gm.mutation_date DESC;
END;

SELECT * FROM good_mutation;