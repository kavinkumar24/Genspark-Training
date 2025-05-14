# SQL DUMP

## Dump

### Restoring the Dump

  - use psql to restore the dump
  
  ``` sql 
  psql -U <username> -d <database_name> -f <dump_file.sql>
  ```

  - Ideal for smaller db

### Using pg_dumpall

- pg_dump dumps only a single database at a time, so to support multi database dumping `pg_dumpall` is provided

``` 
pg_dumpall -U <username> > <dump_file.sql>
```

- Back up all the schema, roles, dbs

``` 
pg_dumpall -U <username> < <dump_file.sql>
```

- restore back into the pgadmin

### Handling Large Databases

- It may challenging for some operating system to dump all the dbs due to its maximum file limits.

### There are some techniques to handle this

- compressed dump `pg_dump dbname | gzip > filename.gz` (using either gzip or zlib) 
- split - `pg_dump dbname | split -b 2G - filename` (which allows to split output into smaller files eg: 2gb chucks)

- To speed up the dump of a `large database`, you can use `pg_dump's parallel mode`. This will dump multiple tables at the same time. You can control the degree of parallelism with the `-j parameter`.

``` 
pg_dump -j num -F d -f out.dir dbname
```

## File System Level Backup 

- An alternative backup strategy is to directly copy the files that PostgreSQL uses to store the data in the database; 
- This approach make a `consistent snapshot`, of the data directory

## Two restricts in this concept

- The database server must be `shut down` in order to get a usable backup.
- The information contained in these files is not usable without the commit log.

## Continuous Archiving and Point-in-Time Recovery (PITR)

### Setting up WAL archiving

- continuously archive WAL logs to enable PITR
- To enable this

  - wal_level = replica
  - archive_mode = on
  - archive_command = 'cp %p /path/to/archive/%f'

-  Upon getting a `zero result`, PostgreSQL will assume that the file has been `successfully archived`, and will remove or recycle it. However, a `nonzero status` tells PostgreSQL that the file was not archived; it will try again periodically until it succeeds.

### Making a Base Backup

- A base backup is a snapshot of the database at a specific point in time
- The easiest way to perform a base backup is to use the `pg_basebackup` tool. It can create a base backup either as regular files or as a tar archive

``` 
pg_basebackup -D /path/to/backup -Ft -z -P
```

- `-D` - specifies destination directory for the backup
- `-Ft` - creates a tarball of the backup (.tar file)
- `-z` - compresses the backup to save space
- `-P` - Displays progress information

### Making an incremental backup 

- It captures only the changes made since the last backup, which optimizing the storage and reducing backup
- run `pg_basebackup` with the `--incremental` option

- start 

`SELECT pg_backup_start(label => 'label', fast => false);
`

- stop

`SELECT * FROM pg_backup_stop(wait_for_archive => true);
`

###  Backing Up the Data Directory

- use tools like `rsync` or `tar` to copy the entire data directory to backup location
- Also exclude some of the directories
- Any file or directory beginning with `pgsql_tmp` can be omitted from the backup. These files are removed on postmaster start and the directories will be recreated as needed.

### Recovering Using a Continuous Archive Backup

- Stop the server
- Restore base backup
- apply archived WAL files
- configure recovery
- start server
- monitor recovery