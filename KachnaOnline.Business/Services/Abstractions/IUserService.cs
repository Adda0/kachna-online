// IUserService.cs
// Author: Ondřej Ondryáš

using System.Collections.Generic;
using System.Threading.Tasks;
using KachnaOnline.Business.Models.Users;
using KachnaOnline.Business.Exceptions.Roles;
using KachnaOnline.Business.Exceptions;

namespace KachnaOnline.Business.Services.Abstractions
{
    public interface IUserService
    {
        /// <summary>
        /// Exchanges a KIS session ID for a KIS authentication token, fetches KIS user data, synchronizes
        /// the user with the local database and returns a JWT Bearer token representing the user's identity.
        /// </summary>
        /// <param name="kisSessionId">A KIS session ID.</param>
        /// <returns>A <see cref="LoginResult"/> structure with the JWT Bearer token or information about errors.</returns>
        Task<LoginResult> LoginSession(string kisSessionId);

        /// <summary>
        /// Exchanges a KIS refresh token for a new KIS authentication token, fetches KIS user data, synchronizes
        /// the user with the local database and returns a JWT Bearer token representing the user's identity.
        /// </summary>
        /// <remarks>
        /// If KIS returns a 403 Forbidden (login not allowed) response, it means that the user is not a member anymore.
        /// In that case, a <see cref="LoginResult"/> with <see cref="LoginResult.UserFound"/> set to false is returned.
        /// </remarks>
        /// <param name="kisRefreshToken">A KIS refresh token.</param>
        /// <returns>A <see cref="LoginResult"/> structure with the JWT Bearer token or information about errors.</returns>
        Task<LoginResult> LoginToken(string kisRefreshToken);

        /// <summary>
        /// Returns the user corresponding to the specified <paramref name="userId"/>.
        /// </summary>
        /// <param name="userId">The user ID to search for.</param>
        /// <returns>A <see cref="User"/> object containing the user matching the specified <paramref name="userId"/>
        /// if such user exists, or null if it doesn't.</returns>
        Task<User> GetUser(int userId);

        /// <summary>
        /// Returns the names of roles assigned to the user corresponding to the specified <paramref name="userId"/>.
        /// </summary>
        /// <param name="userId">The user ID to search for.</param>
        /// <returns>An enumerable of names of roles assigned to the user matching the specified <paramref name="userId"/>
        /// if such user exists, or null if it doesn't.</returns>
        Task<IEnumerable<string>> GetUserRoles(int userId);

        /// <summary>
        /// Returns <see cref="RoleAssignment"/> objects describing the role assignments for the user corresponding
        /// to the specified <paramref name="userId"/>.
        /// </summary>
        /// <param name="userId">The user ID to search for.</param>
        /// <returns>An enumerable of <see cref="RoleAssignment"/> objects describing the role assignments for the user
        /// matching the specified <paramref name="userId"/> if such user exists, or null if it doesn't.</returns>
        Task<IEnumerable<RoleAssignment>> GetUserRoleDetails(int userId);

        /// <summary>
        /// Checks whether the user corresponding to the specified <paramref name="userId"/> has the role specified
        /// by <paramref name="role"/>.
        /// </summary>
        /// <param name="userId">The user ID to check the role for.</param>
        /// <param name="role">The name of the role to check.</param>
        /// <returns>Null if such user doesn't exist. True if the user has the specified role. False otherwise.</returns>
        Task<bool?> IsInRole(int userId, string role);

        /// <summary>
        /// Returns all roles present in the system.
        /// </summary>
        /// <returns>An enumerable of all roles.</returns>
        Task<IEnumerable<Role>> GetRoles();

        /// <summary>
        /// Returns a role with the given ID.
        /// </summary>
        /// <param name="id">ID of the role to return.</param>
        /// <returns>A <see cref="Role"/> with ID <paramref name="id"/>.</returns>
        /// <exception cref="RoleNotFoundException">No such role exists.</exception>
        Task<Role> GetRole(int id);

        /// <summary>
        /// Returns all users.
        /// </summary>
        /// <returns>An enumerable of all users.</returns>
        Task<IEnumerable<User>> GetUsers();

        /// <summary>
        /// Assigns a role to a user.
        /// </summary>
        /// <param name="assignment">The role assignment to add.</param>
        /// <exception cref="RoleAlreadyAssignedException">When the role has already been assigned.</exception>
        /// <exception cref="RoleManipulationFailedException">When the assignment failed.</exception>
        Task AssignRole(UserRole assignment);

        /// <summary>
        /// Revokes a role from a user.
        /// </summary>
        /// <param name="userId">ID of the user to revoke the role from.</param>
        /// <param name="roleId">ID of the role to revoke from the user.</param>
        /// <exception cref="RoleNotFoundException">When the assignment to revoke was not found.</exception>
        /// <exception cref="RoleManipulationFailedException">When the revocation failed.</exception>
        Task RevokeRole(int userId, int roleId);
    }
}
