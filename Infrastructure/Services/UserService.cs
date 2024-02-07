using Azure.Identity;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Services;

public class UserService(UserRepository userRepository)
{
    //Läs ut repo
    private readonly UserRepository _userRepository = userRepository;

    //skapa metod för att skapa en kontakt, skicka med Dto
    //om kontakten inte finns, skapa kontakten. Om Kontakten finns, hämta kontakten och läs ut.

    public async Task<UserEntity> CreateUserAsync(UserDto userRegistrationDto)
    {
        try
        {
            if (!await _userRepository.ExistingAsync(x => x.Email == userRegistrationDto.Email))
            {
                var existingUser = new UserEntity
                {
                    UserName = userRegistrationDto.UserName,
                    Email = userRegistrationDto.Email,
                    Password = userRegistrationDto.Password,
                };
                var newUser = await _userRepository.CreateAsync(existingUser);
                return newUser;
            }
            else
            {
                var existingUser = await _userRepository.GetAsync(x => x.Email == userRegistrationDto.Email);
                return existingUser;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    //skapa metod för att hämta en User
    //Hämta usern om den finns
    public async Task<UserEntity> GetUser(UserDto dto)
    {
        try
        {
            var userEntity = await _userRepository.GetAsync(x => x.UserName == dto.UserName);
            if (userEntity != null)
            {
                return userEntity;
            }              
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    /// <summary>
    /// Gets a list of the existing users in the database
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<UserEntity>> GetUsers()
    {
        try
        {
            var users = await _userRepository.GetAllAsync();
            if(users != null)
            {
                return users;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }


    public async Task<bool> UpdateUser(UserDto dto)
    {
        //hitta och uppdatera

        //skapa en ny user för uppdatering
        var updatedUser = new UserEntity
        {
            UserName = dto.UserName,
            Email = dto.Email,
            Password = dto.Password,
        };

        //spara in den uppdaterade informationen till usern
        var newUser = await _userRepository.UpdateAsync(x => x.UserName == dto.UserName, updatedUser);
        if (newUser != null)
        {   //skapa en ny DTO med den uppdaterade usern
            var userDto = new UserDto
                (
                       newUser.UserName,
                       newUser.Email,
                       newUser.Password
                );
            return userDto != null;
        }
        return false;
    }


    //skapa en metod för att ta bort en User
    //1. gör en sökning, Finns den här användarens email?
    //2. finns den , ta bort och returnera true.
    //3. finns den ej, return false.
    public async Task<bool> DeleteUserAsync(Expression<Func<UserEntity, bool>> expression)
    {
        try
        {
            var result = await _userRepository.DeleteAsync(expression);
            return result;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return false;
    }
}
