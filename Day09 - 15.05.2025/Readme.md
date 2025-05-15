# Day 9

## Sessions

### Morning 

- Security in the application
- Encryption (pgcrypto)
- authentication
    (pg_hba.conf)
    - md5 - password
    - scram-sha-256
    - peer
    - certification
    - trust
- GRANT — privileges

### Afternoon

- Task based on encryption
- Grant options

## Security 

- It about to give a access to a specific resources for the intended people who have that rights

- It is a most important concept in developing the application

## Encryption

- It is a process of transforming readable data into unreadable format to protect it from unauthorized access
- In postgres, there is a method called `pgcrypto` which gives a methods for encryption and decryption

### Symmetric - `pgp_sym_encrypt`, `pgp_sym_decrypt`

- Same key is used to encrypt and decrypt. Simple and fast.

### Asymmetric - `pgp_pub_encrypt`, `pgp_pub_decrypt`

- Public key encrypts, private key decrypts. Used in secure communications.

### Hashing - `digest`, `crypt`, `gen_salt`

- One-way encryption. Can’t be decrypted (used for passwords, checksums, etc).

## Authentication

- `md5` - Password based authentication
- `scram-sha-256` - Hashing and custom defined algo, bit more complex
- `peer` - linux based user authentication
- `cert` - SSL based authentication
- `trust` - Everyone can able to access

## GRANT 

- It is about to give a certain privileges for accessing the data in the database
- Two functions like `GRANT` and `REVOKE` which are able to enable and disable the access
