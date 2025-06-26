# 1. You have a file with permissions -rw-r--r--, and you run chmod +x file.sh. What happens?


- ls -latr
- rw-r--r--    1 root     root             0 Jun 26 13:05 task.txt

## Run command 
chmod +x task.txt

_This adds execute permission to all the users (owner, group, others)_

## Result
```
- rwxr-xr-x    1 root     root             0 Jun 26 13:05 task.txt
    
    - rwx -> Owner (read,write, execute)
    - r-x -> Group (read, execute)
    - r-x -> Group (read,execute)
```


# 2. What is the difference between chmod 744 file.txt and chmod u=rwx,go=r file.txt?

## Commands

- touch file.txt
- chmod 744 file.txt
- ls -latr
## Result
```
-rwxr--r--    1 root     root             0 Jun 26 13:14 file.txt
```

- chmod u=rwx,go=r file.txt
- ls -latr

## Result
```
-rwxr--r--    1 root     root             0 Jun 26 13:14 file.txt
```

## Both the commands are same, but just in different formats, (Symbolic, Numeric)

# 3. What is the sticky bit, and when should you use it?

- Sticky bit is a kind of permission setting in the directory to maintain the security
- Only the owners inside that directory can delete or rename
- For other users who has a permission is able to create and modify files with in that directy (but not delete or rename the files)


## Usecase

- mkdir sharedDir

- ls -latr

``` drwxr-xr-x    2 root     root            37 Jun 26 14:00 sharedDir```

- chmod +t sharedDir   (or)  chmod 1777 sharedDir

``` drwxr-xr-t    2 root     root            37 Jun 26 14:00 sharedDir```

-> `t` represents the sticky bit enabled


# 4. You are told to give the owner full access, group only execute, and others no permissions. What symbolic command achieves this?

- touch permission.txt
(default permission)
`-rw-r--r--    1 root     root             0 Jun 26 14:06 permission.txt`

- chmod u=rwx,g=x,o= permission.txt

`-rwx--x---    1 root     root             0 Jun 26 14:06 permission.txt`

- owner - read, write, execute (full access -> rwx)
- group - execute (x)
- others - none


# 5. What is umask, and why is it important?

- The umask command in Linux which represets the `user file creation mode `
- It **controls the default permissions** for new files and directory created by the user
- It defines which permission bits to subtract

eg: 777 - 022 
    Final -> rwxr-xr-x

# 6. If the umask is 022, what are the default permissions for a new file and a new directory?

#### umask - 0 2 2 
#### permission need to subtract
- user - 0 - subtract nothing
- group - 2 - subtract only write
- others - 2 - subtract write

#### Default permission for new file 
- Default permission without umask - `666`
- Umask with `022`  
- `666-022` - `644` -> `Owner: read and write, Group: read, Others: read`
 
```
rw-r--r--
```
### Default permission for new folder
- Default permission without mask - `777`
- umask with `022`
- `777-022` -> `755` ->`Owner: read, write, execute, Group: read, execute, Others: read and execute`

```
drwxr-xr-x
```

# 7. Why is umask often set to 002 in development environments but 027 or 077 in production?

- The umask for development is `002` 
    - Best when there is a team working 
    - others users in the same group (members of same group) - can read, write, execute
    - outside of the group (only can able to read)
- The umask - 027 or 077
    - 027 means
        - Group has read only, others have no access
    - 077 means
        - Only owner has a permission 

## Reason
- Maintain security in productions
- limit the resource accessing

# 8. useradd vs adduser

### useradd
- useradd is a Linux command for creating a new user.
- It is a low-level command
- No default home directory created (user need to give -m flag)
- need to give -> sudo passwd user (manual entry)

### addUser
- The adduser command creates a new user on a Linux system through an interactive prompt
- Home directory is created automatically