Output on the root of MusicStore:

```
MusicStore Dependency Information
Framework: .NETCoreApp1.0
--------------------------------------------------------------------------------------------------------------
DependencyName                                          | Type       | Requested Version    | Resolved Version
--------------------------------------------------------------------------------------------------------------
Microsoft.NETCore.App                                   | platform   | 1.0.0-*              | 1.0.0
Microsoft.AspNetCore.Authentication.Cookies             | default    | 1.1.0-*              | 1.1.0-alpha1-21772
Microsoft.AspNetCore.Authentication.Facebook            | default    | 1.1.0-*              | 1.1.0-alpha1-21772
Microsoft.AspNetCore.Authentication.Google              | default    | 1.1.0-*              | 1.1.0-alpha1-21772
Microsoft.AspNetCore.Authentication.MicrosoftAccount    | default    | 1.1.0-*              | 1.1.0-alpha1-21772
Microsoft.AspNetCore.Authentication.OpenIdConnect       | default    | 1.1.0-*              | 1.1.0-alpha1-21772
Microsoft.AspNetCore.Authentication.Twitter             | default    | 1.1.0-*              | 1.1.0-alpha1-21772
Microsoft.AspNetCore.Diagnostics                        | default    | 1.1.0-*              | 1.1.0-alpha1-21772
Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore    | default    | 1.1.0-*              | 1.1.0-alpha1-21772
Microsoft.AspNetCore.Identity.EntityFrameworkCore       | default    | 1.1.0-*              | 1.1.0-alpha1-21772
Microsoft.AspNetCore.Mvc                                | default    | 1.1.0-*              | 1.1.0-alpha1-21772
Microsoft.AspNetCore.Mvc.TagHelpers                     | default    | 1.1.0-*              | 1.1.0-alpha1-21772
Microsoft.AspNetCore.Server.IISIntegration              | default    | 1.1.0-*              | 1.1.0-alpha1-21772
Microsoft.AspNetCore.Server.Kestrel                     | default    | 1.1.0-*              | 1.1.0-alpha1-21772
Microsoft.AspNetCore.Server.WebListener                 | default    | 0.2.0-*              | 0.2.0-alpha1-21772
Microsoft.AspNetCore.Session                            | default    | 1.1.0-*              | 1.1.0-alpha1-21772
Microsoft.AspNetCore.StaticFiles                        | default    | 1.1.0-*              | 1.1.0-alpha1-21772
Microsoft.EntityFrameworkCore.InMemory                  | default    | 1.1.0-*              | 1.1.0-alpha1-21772
Microsoft.EntityFrameworkCore.SqlServer                 | default    | 1.1.0-*              | 1.1.0-alpha1-21772
Microsoft.Extensions.Configuration.CommandLine          | default    | 1.1.0-*              | 1.1.0-alpha1-21772
Microsoft.Extensions.Configuration.EnvironmentVariables | default    | 1.1.0-*              | 1.1.0-alpha1-21772
Microsoft.Extensions.Configuration.Json                 | default    | 1.1.0-*              | 1.1.0-alpha1-21772
Microsoft.Extensions.Logging.Console                    | default    | 1.1.0-*              | 1.1.0-alpha1-21772
Microsoft.Extensions.Options.ConfigurationExtensions    | default    | 1.1.0-*              | 1.1.0-alpha1-21772

Diagnostics

Error
--------------------------------------------------------------------------------------------------------------
NONE
--------------------------------------------------------------------------------------------------------------

Warning
--------------------------------------------------------------------------------------------------------------
NONE
--------------------------------------------------------------------------------------------------------------

Info
--------------------------------------------------------------------------------------------------------------
NONE
--------------------------------------------------------------------------------------------------------------


Configured NuGet Feeds:
--------------------------------------------------------------------------------------------------------------
Feed                                                         | From Config
--------------------------------------------------------------------------------------------------------------
https://api.nuget.org/v3/index.json                          | /Users/glennc/.nuget/NuGet/NuGet.Config
https://www.myget.org/F/aspnetcidev/api/v3/index.json        | /Users/glennc/code/glennc/MusicStore/NuGet.Config
https://api.nuget.org/v3/index.json                          | /Users/glennc/code/glennc/MusicStore/NuGet.Config

```

Output on the linq package of MusicStore:

```
MusicStore Dependency Information
Framework: .NETCoreApp1.0
--------------------------------------------------------------------------------------------------------------
Dependency: Remotion.Linq
Type: Package
Selected Version: 2.1.1
Expected Location: /Users/glennc/.nuget/packages/Remotion.Linq/2.1.1
Resolved: ✓

Depended on by:
----------------------------------------------------------------------------------------------------
Top Level  | Parent                                                       | Requested Version
----------------------------------------------------------------------------------------------------
           | Microsoft.EntityFrameworkCore                                | 2.1.1
-----------------------------------------------------------------------------------------------

Depends on:
-----------------------------------------------------------------------------------------------
Dependency                                              | Version                   | Resolved
-----------------------------------------------------------------------------------------------
System.Collections                                      | 4.0.11                    |  ✓
System.Diagnostics.Debug                                | 4.0.11                    |  ✓
System.Linq                                             | 4.1.0                     |  ✓
System.Linq.Expressions                                 | 4.1.0                     |  ✓
System.Linq.Queryable                                   | 4.0.1                     |  ✓
System.ObjectModel                                      | 4.0.12                    |  ✓
System.Reflection                                       | 4.1.0                     |  ✓
System.Reflection.Extensions                            | 4.0.1                     |  ✓
System.Runtime                                          | 4.1.0                     |  ✓
System.Runtime.Extensions                               | 4.1.0                     |  ✓
System.Threading                                        | 4.0.11                    |  ✓
-----------------------------------------------------------------------------------------------
```
