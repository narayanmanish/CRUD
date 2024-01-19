using BusinessAccessLayer.Abstract;
using CRUD18_021_24.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelAccessLayer.ViewModel;
using System.Diagnostics;
using System.Security.Principal;

namespace CRUD18_021_24.Controllers
{
	
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IService _service;
		public HomeController(ILogger<HomeController> logger,IService service)
		{
			_logger = logger;
			_service = service;
		}
		[AllowAnonymous]
		public IActionResult Index()
		{
			return View();
		}
        #region
        [AllowAnonymous]
        public IActionResult Registration()
		{
			return View();
		}
        [HttpPost]
        public async Task<IActionResult> logout()
        {
            var result = await _service.Logout();
            // if(result.Equals(true))
            return RedirectToAction("Login");

        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
			try
			{
                if (ModelState.IsValid)
                {

                    var result = await _service.IsLogin(viewModel);

                    if (result.Equals(true))
                    {
                        return RedirectToAction("GetAllProduct");
                    }

                }
                return View(viewModel);
            }
			catch (Exception)
			{

				throw;
			}
        }
        [HttpPost]
        public async Task<IActionResult> Registration(RegisterViewModel registerViewModel)
        {
			try
			{
				
                if (ModelState.IsValid)
                {

                    bool result = await _service.UserRegister(registerViewModel);

                    if (result.Equals(true))
                    {
                        TempData["SucessMessage"] = "Welcome Login Successfully...";
                        return RedirectToAction("GetAllProduct");
                    }
					else
					{
                        TempData["ErrorMessage"] = "Wrong Id Or Password...";
                    }
                  
                }
                return View();
            }
			catch (Exception)
			{

				throw;
			}
            
        }

        #endregion


        #region
        [HttpGet]
		public IActionResult Product()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Product(ProductViewModel productViewModel)
		{
			try
			{
				if(ModelState.IsValid)
				{
					bool result=_service.addProduct(productViewModel);
					if(result)
					{
						TempData["SucessMessage"] = "Product Added Successfully...";
						return RedirectToAction("GetAllProduct");
					}
					else
					{
                        TempData["ErrorMessage"] = "Product Not Added...";
                    }
                  
                }
                return View();
            }
			catch (Exception)
			{

				throw;
			}
			
		}
		public IActionResult GetAllProduct()
		{
			try
			{
				var ProductList = _service.getProducts();
				if(ProductList != null)
				{
					return View(ProductList);
				}
			}
			catch (Exception)
			{

				throw;
			}
			return View();
		}
		public IActionResult EditProduct(int id)
		{
			try
			{
				var product = _service.ProductGetbyId(id);
				if(product != null)
				{
					return View(product);
				}

            }
			catch (Exception)
			{

				throw;
			}
			return View();
		}
		[HttpPost]
		public IActionResult EditProduct(ProductViewModel productViewModel)
        {
			try
			{
				if(ModelState.IsValid)
				{
					bool result = _service.UpdateProduct(productViewModel);
                    if (result)
                    {
                        TempData["SucessMessage"] = "Product Updated Successfully...";
                        return RedirectToAction("GetAllProduct");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Product Not Updated...";
                    }
                   
                }
                return View();
            }
			catch (Exception)
			{

				throw;
			}
		}
		[HttpGet]
		public IActionResult Delete(int id)
		{
			try
			{
				var result = _service.ProductGetbyId(id);
				if(result != null)
				{
					return View(result);
				}
			}
			catch (Exception)
			{

				throw;
			}
			return View();
		}
		[HttpPost]
        public IActionResult Delete(int id,ProductViewModel productViewModel)
        {
            try
            {
				bool result = _service.ProductDelete(id);
                if (result)
                {
                    TempData["SucessMessage"] = "Product Deleted Successfully...";
                    return RedirectToAction("GetAllProduct");
                }
                else
                {
                    TempData["ErrorMessage"] = "Product Not Deleted...";
                }
				return View();
            }
            catch (Exception)
            {

                throw;
            }
          
        }
        #endregion
        public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}