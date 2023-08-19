using Extenso.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;

namespace Extenso.AspNetCore.OData.Tests
{
    // https://github.com/bUnit-dev/bUnit/blob/c8202c414f55a4070b109654f00ce462297a8302/src/bunit.web/TestDoubles/Authorization/FakeAuthorizationService.cs#L13
    public class FakeAuthorizationService : IAuthorizationService
    {
        private AuthorizationState currentState;
        private IEnumerable<string>? supportedPolicies;
        private IEnumerable<string> supportedRoles = Array.Empty<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeAuthorizationService"/> class.
        /// </summary>
        /// <param name="state">Initial authorization state.</param>
        public FakeAuthorizationService(AuthorizationState state = AuthorizationState.Authorized)
        {
            currentState = state;
        }

        /// <summary>
        /// Method to explicitly set the authorization state.
        /// </summary>
        /// <param name="state">New state to set.</param>
        public void SetAuthorizationState(AuthorizationState state)
        {
            currentState = state;
        }

        /// <summary>
        /// Sets the supported policies in the Authorization service for current user.
        /// </summary>
        /// <param name="policies">List of supported policies.</param>
        public void SetPolicies(IEnumerable<string> policies)
        {
            supportedPolicies = policies;
        }

        /// <summary>
        /// Sets the supported roles in the Authorization service for current user.
        /// </summary>
        /// <param name="roles">List of supported roles.</param>
        public void SetRoles(IEnumerable<string> roles)
        {
            supportedRoles = roles ?? Array.Empty<string>();
        }

        /// <summary>
        /// Authorizes the Principal user based solely on the current authorization state.
        /// </summary>
        /// <param name="user">Principal user to authorize.</param>
        /// <param name="resource">Resource being authorized.</param>
        /// <param name="requirements">Authorization requirements.</param>
        /// <returns>Result of authorize request.</returns>
        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            if (requirements is null)
                throw new ArgumentNullException(nameof(requirements));

            AuthorizationResult result;

            var requirementsArray = requirements.ToArray();
            if (Array.TrueForAll(requirementsArray, p => p is DenyAnonymousAuthorizationRequirement))
            {
                result = currentState == AuthorizationState.Authorized
                    ? AuthorizationResult.Success()
                    : AuthorizationResult.Failed();
            }
            else if (Array.TrueForAll(requirementsArray, p => p is RolesAuthorizationRequirement))
            {
                result = VerifyRequiredRoles(requirementsArray);
            }
            else if (supportedPolicies is not null)
            {
                result = VerifyRequiredPolicies(requirementsArray);
            }
            else
            {
                result = AuthorizationResult.Failed();
            }

            return Task.FromResult(result);
        }

        /// <summary>
        /// Authorizes the Principal user based solely on the current authorization state.
        /// </summary>
        /// <param name="user">Principal user to authorize.</param>
        /// <param name="resource">Resource being authorized.</param>
        /// <param name="policyName">Policy to use for authorization.</param>
        /// <returns>Result of authorize request.</returns>
        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, string policyName)
        {
            var requirements = new List<IAuthorizationRequirement>
            {
                new TestPolicyRequirement { PolicyName = policyName },
            };

            return AuthorizeAsync(user, resource, requirements);
        }

        private AuthorizationResult VerifyRequiredPolicies(IReadOnlyCollection<IAuthorizationRequirement> requirements)
        {
            if (supportedPolicies.IsNullOrEmpty() || requirements.IsNullOrEmpty())
            {
                return AuthorizationResult.Failed();
            }

            return requirements.OfType<TestPolicyRequirement>().Any(req => supportedPolicies.Contains(req.PolicyName, StringComparer.Ordinal))
                ? AuthorizationResult.Success()
                : AuthorizationResult.Failed();
        }

        private AuthorizationResult VerifyRequiredRoles(IReadOnlyCollection<IAuthorizationRequirement> requirements)
        {
            var result = AuthorizationResult.Failed();
            foreach (var req in requirements.OfType<RolesAuthorizationRequirement>())
            {
                var rolesFound = req.AllowedRoles.Intersect(supportedRoles, StringComparer.Ordinal);
                if (rolesFound.Any())
                {
                    result = AuthorizationResult.Success();
                    break;
                }
            }

            return result;
        }
    }

    /// <summary>
    /// Enumeration that represents the user's authorization state.
    /// </summary>
    public enum AuthorizationState
    {
        /// <summary>
        /// Represents unauthorized user state.
        /// </summary>
        Unauthorized,

        /// <summary>
        /// Represents authorized user state.
        /// </summary>
        Authorized,

        /// <summary>
        /// Represents authorizing user state.
        /// </summary>
        Authorizing,
    }

    /// <summary>
    /// Test requirement that supports the minimum requirement - just a policy name.
    /// </summary>
    public class TestPolicyRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// Gets or sets the policy name for this requirement.
        /// </summary>
        public string PolicyName { get; set; } = string.Empty;
    }
}