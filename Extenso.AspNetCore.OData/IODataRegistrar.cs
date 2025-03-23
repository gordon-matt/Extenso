using Microsoft.AspNetCore.OData;

namespace Extenso.AspNetCore.OData;

/// <summary>
/// Defines a contract for an OData registrar in an application. An OData registrar specifies
/// the OData routes for an application.
/// </summary>
public interface IODataRegistrar
{
    /// <summary>
    /// Registers one or more OData routes for use in the application.
    /// </summary>
    /// <param name="options">The OData options used to configure the services with.</param>
    void Register(ODataOptions options);
}