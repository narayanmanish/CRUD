using DataAccessLayer.AppDb;
using ModelAccessLayer.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer.Abstract
{
	public interface IService
	{
		bool addProduct(ProductViewModel productViewModel);
		IEnumerable<Product> getProducts();
		ProductViewModel ProductGetbyId(int id);
		bool UpdateProduct(ProductViewModel productViewModel);
		bool ProductDelete(int id);
		Task<bool> UserRegister(RegisterViewModel registerViewModel);
		Task<bool> Logout();
		Task<bool> IsLogin(LoginViewModel viewModel);

    }
}
