


-- Database: `Malshinon`


CREATE TABLE People (
	id int NOT NULL PRIMARY KEY AUTO_INCREMENT,
	first_name varchar(100) NOT NULL,
	last_name varchar(150) NOT NULL,
	secret_code varchar(50) UNIQUE,
	type ENUM ("reporter", "target", "both", "potential_agent"),
	num_reports int default 0,
	num_mentions int default 0
);


CREATE TABLE IntelReports (
	id int NOT NULL PRIMARY KEY AUTO_INCREMENT,
	reporter_id int,
	tarrget_id int,
	FOREIGN KEY (reporter_id) REFERENCES People(id),
	FOREIGN KEY (tarrget_id) REFERENCES People(id),
	text TEXT,
	timestamp datetime default NOW()
);