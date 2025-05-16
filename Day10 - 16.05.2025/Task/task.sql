-- You are tasked with building a PostgreSQL-backed database for an EdTech company that manages 
-- online training and certification programs for individuals across various technologies.

-- The goal is to:

-- Design a normalized schema

-- Support querying of training data

-- Ensure secure access

-- Maintain data integrity and control over transactional updates

-- Database planning (Nomalized till 3NF)

-- A student can enroll in multiple courses

-- Each course is led by one trainer

-- Students can receive a certificate after passing

-- Each certificate has a unique serial number

-- Trainers may teach multiple courses

-- Tables to Design (Normalized to 3NF):
-- * Create all tables with appropriate constraints (PK, FK, UNIQUE, NOT NULL)

-- 1. **students**

--    * `student_id (PK)`, `name`, `email`, `phone`

-- 2. **courses**

--    * `course_id (PK)`, `course_name`, `category`, `duration_days`

-- 3. **trainers**

--    * `trainer_id (PK)`, `trainer_name`, `expertise`

-- 4. **enrollments**

--    * `enrollment_id (PK)`, `student_id (FK)`, `course_id (FK)`, `enroll_date`

-- 5. **certificates**

--    * `certificate_id (PK)`, `enrollment_id (FK)`, `issue_date`, `serial_no`

-- 6. **course\_trainers** (Many-to-Many if needed)

--    * `course_id`, `trainer_id`
--------------------------------------------------------------------------
-- Phase 2: DDL & DML

-- * Create all tables with appropriate constraints (PK, FK, UNIQUE, NOT NULL)

CREATE TABLE students (
	student_id SERIAL CONSTRAINT pk_student_id PRIMARY KEY,
	name TEXT NOT NULL,
	email VARCHAR(100) UNIQUE NOT NULL,
	phone CHAR(10) UNIQUE  NOT NULL CHECK (phone ~ '^\d{10}$')
);

-------------------------------------------------------------
CREATE TABLE courses (
	course_id SERIAL CONSTRAINT pk_course_id PRIMARY KEY,
	course_name TEXT NOT NULL,
	category TEXT NOT NULL,
	duration_days INT NOT NULL
);
--------------------------------------------------------------
CREATE TABLE trainers(
	trainer_id SERIAL CONSTRAINT pk_trainer_id PRIMARY KEY,
	trainer_name TEXT NOT NULL,
	expertise TEXT NOT NULL
);
---------------------------------------------------------------
CREATE TABLE enrollments(
	enrollment_id SERIAL CONSTRAINT pk_enroll_id PRIMARY KEY,
	student_id INT NOT NULL,
	course_id  INT NOT NULL,
	enroll_date DATE NOT NULL,
	FOREIGN KEY (student_id) REFERENCES students(student_id) ON DELETE CASCADE,
	FOREIGN KEY (course_id) REFERENCES courses(course_id) ON DELETE CASCADE
);

ALTER TABLE enrollments
ADD COLUMN status boolean;
---------------------------------------------------------------
CREATE TABLE certificates(
	certificate_id SERIAL CONSTRAINT pk_cert_id PRIMARY KEY,
	enrollment_id INT NOT NULL,
	issue_date DATE NOT NULL,
	serial_no VARCHAR(100) UNIQUE NOT NULL,
	FOREIGN KEY (enrollment_id) REFERENCES enrollments(enrollment_id) ON DELETE CASCADE
);
----------------------------------------------------------------
CREATE TABLE course_trainers(
	course_id INT NOT NULL,
	trainer_id INT NOT NULL,
	PRIMARY KEY (course_id, trainer_id),
	FOREIGN KEY (course_id) REFERENCES courses (course_id),
	FOREIGN KEY (trainer_id) REFERENCES trainers(trainer_id)
);

------------------------------------------------------------------


-- * Insert sample data using `INSERT` statements

INSERT INTO students(name, email, phone)
VALUES 
('Ramu', 'ramusample@gmail.com', 1234567890),
('somu', 'somusample@gmail.com', 1234567891),
('vembu', 'vembusample@gmail.com', 1234567892),
('sanjiv', 'sanjivsample@gmail.com', 1234567893)

SELECT * FROM students

INSERT INTO courses(course_name, category, duration_days)
VALUES 
('Sql-Intermediate', 'Programming', 20),
('Calcus and integration', 'Maths', 50),
('C-programming', 'Programming', 30),
('Machine learning Basics', 'Data Science', 60)
('Java', 'Programming', 70)

INSERT INTO trainers(trainer_name,expertise)
VALUES 
('Alice Bob', 'Python'),
('Harthiesh Krishna', 'Javascript, web dev'),
('Ram guru', 'Maths'),
('Siva', 'Sql, C-programming, C++, C#'),
('Ram', 'Machine learning and Data science')


INSERT INTO enrollments(student_id, course_id, enroll_date)
VALUES
(1, 1, CURRENT_DATE),
(1, 4, CURRENT_DATE - INTERVAL '4 days'),
(2, 3, CURRENT_DATE - INTERVAL '3 days'),
(4, 2, CURRENT_DATE - INTERVAL '1 days')
(2, 1, CURRENT_DATE)

INSERT INTO certificates (enrollment_id, issue_date, serial_no)
VALUES 
(1, CURRENT_DATE, 12345001),
(2, CURRENT_DATE - INTERVAL '1 days', 12345002),
(3, CURRENT_DATE - INTERVAL '1 days', 12345003),
(4, CURRENT_DATE, 12345004)

INSERT INTO course_trainers(course_id, trainer_id)
VALUES
(1,4),
(2,3),
(3,4),
(4,5)


SELECT * FROM students;
SELECT * FROM trainers;
SELECT * FROM courses;
SELECT * FROM enrollments;
SELECT * FROM course_trainers;
-- * Create indexes on `student_id`, `email`, and `course_id`

-- indexes on students
CREATE INDEX idx_student_id ON students(student_id);
CREATE UNIQUE INDEX idx_student_email ON students(email);

-- indexes on enrollment
CREATE INDEX idx_enrollment_student_id ON enrollments(student_id);
CREATE INDEX idex_enrollment_course_id ON enrollments(course_id);

-- index on course
CREATE INDEX idx_course_id ON courses(course_id);

---------------------------------------------------------------------------------------------------------

-- Phase 3: SQL Joins Practice

-- Write queries to:

SELECT * from enrollments
-- 1. List students and the courses they enrolled in
SELECT s.student_id, s.name, c.course_name
FROM students s 
LEFT JOIN enrollments e ON s.student_id = e.student_id
LEFT JOIN courses c ON e.course_id = c.course_id

-- 2. Show students who received certificates with trainer names
SELECT * FROM certificates
SELECT * FROM enrollments
SELECT * FROM courses
SELECT * FROM students
SELECT * FROM trainers
SELECT * FROM course_trainers

SELECT cr.certificate_id, cr.issue_date, s.student_id, s.name AS student_name, c.course_name, t.trainer_name
FROM certificates cr
JOIN enrollments e ON cr.enrollment_id = e.enrollment_id
JOIN students s ON e.student_id = s.student_id
JOIN courses c ON e.course_id = c.course_id
JOIN course_trainers ct ON  c.course_id = ct.course_id
JOIN trainers t ON ct.trainer_id = t.trainer_id


-- 3. Count number of students per course
SELECT c.course_name, COUNT(e.student_id) AS total_students
FROM courses c 
LEFT JOIN enrollments e ON c.course_id = e.course_id
GROUP BY c.course_id, c.course_name
ORDER BY total_students DESC;
---------------------------------------------------------------------------------------------------------

-- Phase 4: Functions & Stored Procedures

-- Function:

-- Create `get_certified_students(course_id INT)`
-- → Returns a list of students who completed the given course and received certificates.

SELECT * FROM courses
SELECT * FROM enrollments
SELECT * FROM certificates

CREATE OR REPLACE FUNCTION get_certified_students(p_course_id INT)
RETURNS TABLE (student_id INT, name TEXT, certificate_id INT)
AS $$
BEGIN
	RETURN QUERY  
	SELECT s.student_id, s.name, cr.certificate_id, cr.issue_date
	FROM certificates cr 
    JOIN enrollments e ON cr.enrollment_id = e.enrollment_id
    JOIN students s ON e.student_id = s.student_id
    JOIN courses c ON e.course_id = c.course_id
	WHERE c.course_id = p_course_id;
END;
$$ LANGUAGE plpgsql;

SELECT * FROM enrollments
SELECT * FROM certificates
SELECT * FROM get_certified_students(4);

---------------------------------------------------------------------------------------------------------
-- Stored Procedure:

-- Create `sp_enroll_student(p_student_id, p_course_id)`
-- → Inserts into `enrollments` and conditionally adds a certificate if completed (simulate with status flag).


CREATE OR REPLACE PROCEDURE sp_enroll_student(p_student_id INT, p_course_id INT, p_flag BOOLEAN)
AS $$
DECLARE 
	next_certificate_serial_no BIGINT;
BEGIN
	IF EXISTS (SELECT 1 FROM enrollments WHERE student_id = p_student_id AND course_id = p_course_id) THEN
		IF EXISTS (SELECT 1 FROM certificates WHERE enrollment_id IN 
		(SELECT enrollment_id FROM enrollments WHERE student_id = p_student_id AND 
			course_id = p_course_id)) THEN
			RAISE NOTICE 'Student % already completed the course (course_id - %) and certification received', p_student_id, p_course_id;
			
		ELSEIF p_flag = true THEN
			SELECT COALESCE(MAX(serial_no::BIGINT),0)+1 INTO next_certificate_serial_no FROM certificates;
			INSERT INTO certificates (enrollment_id, issue_date, serial_no)
			SELECT enrollment_id, CURRENT_DATE, next_certificate_serial_no FROM enrollments
			WHERE student_id = p_student_id AND course_id = p_course_id;

			UPDATE enrollments SET status = true 
			WHERE student_id = p_student_id AND course_id = p_course_id;
			
			RAISE NOTICE 'Certificate Issued for the student % for the course_id %', p_student_id,p_course_id;
		ELSE 
			RAISE NOTICE 'Already enrolled but not completed the course';
		END IF;
		
	ELSE
		INSERT INTO enrollments (student_id, course_id, enroll_date, status)
		VALUES (p_student_id, p_course_id, NOW(), False);
		RAISE NOTICE 'Student has been enrolled in a course';
	END IF;
END;
$$ LANGUAGE plpgsql;
		
CALL sp_enroll_student(2, 4, true); 

CALL sp_enroll_student(4, 1, true);

CALL sp_enroll_student(3, 1, false);

CALL sp_enroll_student(3, 1, true);

CALL sp_enroll_student(3, 2, false);

CALL sp_enroll_student(1, 5, false);



SELECT * FROM enrollments
SELECT * FROM certificates

INSERT INTO enrollments(student_id, course_id, enroll_date, status)
VALUES (3, 2, NOW(), false);
--------------------------------------------------------------------------------------------------------- 

-- Phase 5: Cursor

-- Use a cursor to:

-- * Loop through all students in a course
-- * Print name and email of those who do not yet have certificates

CREATE OR REPLACE PROCEDURE sp_without_certification()
AS $$
DECLARE
	row_data RECORD;
	cursor_traverse CURSOR FOR
		SELECT DISTINCT s.name, s.email
		FROM students s
		LEFT JOIN enrollments e ON s.student_id = e.student_id
		LEFT JOIN certificates c ON e.enrollment_id = c.enrollment_id
		WHERE c.certificate_id IS NULL;
BEGIN
	OPEN cursor_traverse;
	LOOP
		FETCH cursor_traverse INTO row_data;
		EXIT WHEN NOT FOUND;
		RAISE NOTICE 'Student: %, Email - %', row_data.name, row_data.email;
	END LOOP;
	CLOSE cursor_traverse;
END;
$$ LANGUAGE plpgsql;

CALL sp_without_certification()

SELECT * FROM enrollments
SELECT * FROM students
SELECT * FROM certificates
---------------------------------------------------------------------------------------------------------

-- Phase 6: Security & Roles

-- 1. Create a `readonly_user` role:

--    * Can run `SELECT` on `students`, `courses`, and `certificates`
--    * Cannot `INSERT`, `UPDATE`, or `DELETE`

 CREATE ROLE readonly_user WITH LOGIN PASSWORD '123'
 
 GRANT CONNECT ON DATABASE EdTech TO readonly_user;
 
 GRANT USAGE ON SCHEMA PUBLIC TO readonly_user;
 
 GRANT SELECT ON students, courses, certificates TO readonly_user;

 REVOKE SELECT ON students, courses, certificates FROM readonly_user;

-- 2. Create a `data_entry_user` role:

--    * Can `INSERT` into `students`, `enrollments`
--    * Cannot modify certificates directly

CREATE ROLE data_entry_user WITH LOGIN PASSWORD '1234';

GRANT CONNECT ON DATABASE EdTech TO data_entry_user;

GRANT USAGE ON SCHEMA PUBLIC TO data_entry_user;

GRANT INSERT ON students, enrollments TO data_entry_user;

REVOKE INSERT ON students, enrollments FROM data_entry_user;
---------------------------------------------------------------------------------------------------------

-- Phase 7: Transactions & Atomicity

-- Write a transaction block that:

-- * Enrolls a student
-- * Issues a certificate
-- * Fails if certificate generation fails (rollback)

-- ```sql
-- BEGIN;
-- -- insert into enrollments
-- -- insert into certificates
-- -- COMMIT or ROLLBACK on error

-- Transaction in stored procedure (solution 1 with condition)

CREATE OR REPLACE PROCEDURE sp_enroll_and_issue_certification (p_student_id INT, p_course_id INT,p_flag TEXT)
AS $$
DECLARE 
    next_certificate_serial_no BIGINT;
BEGIN
    BEGIN 

        IF LOWER(p_flag) = 'enroll' THEN
            IF EXISTS (
                SELECT 1 FROM enrollments 
                WHERE student_id = p_student_id AND course_id = p_course_id
            ) THEN
                RAISE NOTICE 'Student % already enrolled in this course_id - %', p_student_id, p_course_id;
            ELSE
                INSERT INTO enrollments (student_id, course_id, enroll_date, status)
                VALUES (p_student_id, p_course_id, NOW(), FALSE);
                RAISE NOTICE 'Student % has been enrolled in course %', p_student_id, p_course_id;
            END IF;

        ELSIF LOWER(p_flag) = 'certificate' THEN
            IF EXISTS ( SELECT 1 FROM certificates WHERE enrollment_id IN (
                    SELECT enrollment_id FROM enrollments 
                    WHERE student_id = p_student_id AND course_id = p_course_id
                )
            ) THEN
                RAISE NOTICE 'Student % already received a certificate for course_id %', p_student_id, p_course_id;
            ELSE
                SELECT COALESCE(MAX(serial_no::BIGINT),0)+1
                INTO next_certificate_serial_no 
                FROM certificates;

                INSERT INTO certificates (enrollment_id, issue_date, serial_no)
                SELECT enrollment_id, CURRENT_DATE, next_certificate_serial_no 
                FROM enrollments
                WHERE student_id = p_student_id AND course_id = p_course_id;

                RAISE NOTICE 'Certificate issued in email';
            END IF;

        ELSE
            RAISE NOTICE 'Please set the flag to either "enroll" or "certificate"';
        END IF;

    EXCEPTION
        WHEN OTHERS THEN
            RAISE NOTICE 'An error occurred: %', SQLERRM;
            RAISE;
    END;

END;
$$ LANGUAGE plpgsql;

SELECT * FROM certificates

CALL sp_enroll_and_issue_certification(1,1,'enroll');
CALL sp_enroll_and_issue_certification(1,1,'certificate');
CALL sp_enroll_and_issue_certification(1,1,'enrolll');
CALL sp_enroll_and_issue_certification(991,1,'enroll');


-- Transaction block (solution 2)
DO $$
DECLARE 
	v_enrollment_id INT;
BEGIN 
	BEGIN
		INSERT INTO enrollments (studet_id, course_id, enroll_date, status)
		VALUES (6, 2, CURRENT_DATE, TRUE)
		RETURNING enrollment_id INTO v_enrollment_id;

		INSERT INTO certificates (enrollment_id, issue_date, serial_no)
		VALUES (v_enrollment_id, CURRENT_DATE,(SELECT COALESCE(MAX(serial_no),0)+1 FROM certificates));
		COMMIT;
	EXCEPTION WHEN OTHERS THEN
		ROLLBACK;
		RAISE NOTICE 'Transaction failed and rolled back';
	END;
END $$;

---------------------------------------------------------------------------------------------------------
