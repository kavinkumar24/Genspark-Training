
## Terminal commands 

1. docker volume create mydbdata


2. docker run -d \
  --name mysql-container \
  -e MYSQL_ROOT_PASSWORD=root \
  -v mydbdata:/var/lib/mysql \
  mysql:latest


3. docker exec -it mysql-container mysql -uroot -proot


4.  CREATE DATABASE testdb;
    USE testdb;
    CREATE TABLE users(id INT, name VARCHAR(50));
    INSERT INTO users VALUES (1, 'RAMMU');


5. docker stop mysql-container
6. docker rm mysql-container


7. docker run -d \
  --name mysql-container \
  -e MYSQL_ROOT_PASSWORD=root \
  -v mydbdata:/var/lib/mysql \
  mysql:latest


8. docker exec -it mysql-container mysql -uroot -proot


9. USE testdb;
   SELECT * FROM users;


or else we can add the compose file