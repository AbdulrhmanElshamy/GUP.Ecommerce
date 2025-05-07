using GUP.Ecommerce.Abstractions;
using GUP.Ecommerce.Contracts.Roles;

namespace GUP.Ecommerce.RoleServices.Services;

public interface IRoleService
{
    Task<IEnumerable<RoleResponse>> GetAllAsync(bool includeDisabled = false, CancellationToken cancellationToken = default);
    Task<Result<RoleDetailResponse>> GetAsync(string id);
}