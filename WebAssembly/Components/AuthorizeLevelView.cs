using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Rendering;
using Shared.DB.Classes.User;
using WebAssembly.Auth;

namespace WebAssembly.Components;

public class AuthorizeLevelView : ComponentBase
{
    [Parameter] public AccessLevel RequiredLevel { get; set; }
    [Inject] private IAuthorizationService AuthorizationService { get; set; }
    [Inject] private NavigationManager Navigation { get; set; }
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        var access = await AuthorizationService.AuthorizeAsync(user, null, new AccessLevelRequirement(RequiredLevel));

        if (!access.Succeeded)
        {
            Navigation.NavigateTo("/auth");
        }
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        base.BuildRenderTree(builder);
        // Render content only if authorized
    }
}