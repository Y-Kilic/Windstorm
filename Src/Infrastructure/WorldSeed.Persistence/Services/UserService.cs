﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WorldSeed.Application.DTOS;
using WorldSeed.Application.Interfaces;
using WorldSeed.Application.Interfaces.Services;
using WorldSeed.Domain.Entities;

namespace WorldSeed.Persistence.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfwork;

        public UserService(IUnitOfWork unitOfwork)
        {
            _unitOfwork = unitOfwork;
        }

        public bool CreateUser(CreateUserDTO createUserDTO)
        {
            var newUser = new User()
            {
                UserName = createUserDTO.Username,
                Email = createUserDTO.Email,
                PasswordHash = createUserDTO.PasswordHash,
                PasswordSalt = createUserDTO.PasswordSalt
            };

            _unitOfwork.Users.Add(newUser);

            _unitOfwork.SaveChanges();

            return true;
        }

        public User CheckLogin(LoginUserDTO loginUserDTO)
        {
            var userFromDB = _unitOfwork.Users.GetAll().Where(u => u.UserName.Equals(loginUserDTO.UserName)).FirstOrDefault();

            var result = VerifyPasswordHash(loginUserDTO.Password, userFromDB.PasswordHash, userFromDB.PasswordSalt);

            if(result == true)
            {
                return userFromDB;

            }

            return null;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

    }
}
