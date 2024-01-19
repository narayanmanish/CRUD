using BusinessAccessLayer.Abstract;
using DataAccessLayer.AppDb;
using Microsoft.AspNetCore.Identity;
using ModelAccessLayer.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer.Implimentation
{
	public class AccountService : IService
	{
		private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AccountService(ApplicationDbContext applicationDbContext, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
			_context = applicationDbContext;
			_userManager = userManager;
			_signInManager = signInManager;
        }
        /// <summary>
        ///addproduct methos is created for add Product 
        /// </summary>
        /// <param name="productViewModel"></param>
        /// <returns>bool</returns>
        public bool addProduct(ProductViewModel productViewModel)
		{
			try
			{
				var find = _context.Products.Where(x=>x.Name == productViewModel.Name).FirstOrDefault();
				if (find != null) 
				{
					return false;
				}
				else
				{
					Product product = new Product();
					product.Name = productViewModel.Name;
					product.Description = productViewModel.Description;
					product.Price = productViewModel.Price;
					product.CreatedOn = DateTime.Now;
					product.CreatedBy = 1;
					_context.Products.Add(product);
					var result = _context.SaveChanges();
					if(result>0)
					{
						return true;
					}
					return false;
				}
				
			}
			catch (Exception)
			{

				throw;
			}
		}

        public IEnumerable<Product> getProducts()
        {
			try
			{
				var ProductList = _context.Products.ToList();
				if(ProductList!=null)
				{
					return ProductList;
				}
				return null;
			}
			catch (Exception)
			{

				throw;
			}
        }

        public bool ProductDelete(int id)
        {
			try
			{
				var result = _context.Products.Find(id);
				if(result!=null)
				{
					_context.Remove(result);
                    var delete = _context.SaveChanges();
					if(delete>0)
					{
						return true;
					}
				}
				return false;
			}
			catch (Exception)
			{

				throw;
			}
        }

        public ProductViewModel ProductGetbyId(int id)
        {
			try
			{
				var product = _context.Products.Find(id);
				if(product!=null)
				{
					ProductViewModel productViewModel= new ProductViewModel();
					productViewModel.Id = id;
					productViewModel.Name = product.Name;
					productViewModel.Description = product.Description;
					productViewModel.Price = product.Price;
					
					return productViewModel;
				}
				return null;
			}
			catch (Exception)
			{

				throw;
			}
        }

        public bool UpdateProduct(ProductViewModel productViewModel)
        {
			try
			{
				Product product = new Product();
				product.Id = productViewModel.Id;
				product.Name = productViewModel.Name;
				product.Description = productViewModel.Description;
				product.Price = productViewModel.Price;
				_context.Products.Update(product);
                var result = _context.SaveChanges();
				if(result>0)
				{
					return true;
				}
				return false;
			}
			catch (Exception)
			{

				throw;
			}
        }
        public async Task<bool> Logout()
        {
            await _signInManager.SignOutAsync();
            return true;
        }
        public async Task<bool> IsLogin(LoginViewModel viewModel)
        {
            var result = await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.RememberMe, false);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UserRegister(RegisterViewModel registerViewModel)
        {
			try
			{
                var user = new IdentityUser { UserName = registerViewModel.Email, Email = registerViewModel.Email };
                var result = await _userManager.CreateAsync(user, registerViewModel.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return true;
                }

                return false;
            }
			catch (Exception)
			{

				throw;
			}
        }
    }
}
