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
	mutation_date DATETIME,
	status NVARCHAR(10) NOT NULL,
	amount INT NOT NULL,
	CONSTRAINT fk_good FOREIGN KEY (good_id) REFERENCES good(good_id)
);

CREATE INDEX idx_category_name ON category (category_name);
CREATE INDEX idx_supplier_name ON supplier (supplier_name);
CREATE INDEX idx_good_code ON good (good_code);
CREATE INDEX idx_good_name ON good (good_name);
CREATE INDEX idx_good_mutation_date ON good_mutation (mutation_date);

