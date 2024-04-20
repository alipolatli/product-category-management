using Microsoft.AspNetCore.Http;

namespace product_category.core;

public sealed class TenantProvider
{
	private const string TENANT_KEY = "X-TenantId";
	private readonly IHttpContextAccessor _httpContextAccessor;

	public TenantProvider(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
	}

	public Guid GetTenantId()
	{
		Guid? tenantIdFromHeader = GetTenantIdInHeader();

		if (tenantIdFromHeader != null)
			return tenantIdFromHeader.Value;
		return GetTenantIdInJwt() ?? Guid.NewGuid();
	}

	private Guid? GetTenantIdInHeader()
	{
		var tenantIdHeader = _httpContextAccessor.HttpContext?.Request.Headers[TENANT_KEY];
		if (!string.IsNullOrEmpty(tenantIdHeader) && Guid.TryParse(tenantIdHeader, out Guid tenantId))
			return tenantId;
		return null;
	}

	private Guid? GetTenantIdInJwt()
	{
		var tenantIdClaim = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == TENANT_KEY);
		if (tenantIdClaim != null && Guid.TryParse(tenantIdClaim.Value, out Guid tenantId))
			return tenantId;
		return null;
	}
}