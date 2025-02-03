
namespace MB.EventResults.Blazor.Server;

[ApiController]
public class ImportController(ILogger<ImportController> _Logger, IAuthService _AuthService, IImportService _ImportService, ICacheService _CacheService, IProcessedResultService _ProcessedResultService) : Controller {
  [HttpPost(UrlConstants.Import)]
  [ResponseCache(NoStore = true, Duration = 0)]
  public async Task<IActionResult> Import() {
    if (!_AuthService.CheckAuth(Request.Headers)) {
      return Unauthorized();
    }

    var realFiles = Request.Form.Files.Where(p => p.Length > 0).ToList();

    _Logger.LogInformation($"Processing {realFiles.Count} files.");

    bool any = false;

    foreach (var formFile in realFiles) {
      using var stream = formFile.OpenReadStream();
      any = await _ImportService.Import(stream) || any;
    }

    if (any) {
      await _ImportService.Reindex();
      await _CacheService.Clear();
    }

    return any ? Ok() : NotFound();
  }

  [HttpGet(UrlConstants.Clear)]
  [ResponseCache(NoStore = true, Duration = 0)]
  public IActionResult Clear() {
    if (!_AuthService.CheckAuth(Request.Headers)) {
      return Unauthorized();
    }

    _ImportService.Clear();
    _CacheService.Clear();

    return Ok();
  }

  [HttpGet(UrlConstants.ReIndex)]
  [ResponseCache(NoStore = true, Duration = 0)]
  public async Task<IActionResult> Reindex() {
    if (!_AuthService.CheckAuth(Request.Headers)) {
      return Unauthorized();
    }

    await _ImportService.Reindex();
    await _CacheService.Clear();

    return Ok();
  }
}