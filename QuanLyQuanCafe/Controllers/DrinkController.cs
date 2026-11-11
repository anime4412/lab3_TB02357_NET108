using Microsoft.AspNetCore.Mvc;
using QuanLyQuanCafe.DAL;
using QuanLyQuanCafe.Models;

namespace QuanLyQuanCafe.Controllers
{
    public class DrinkController : Controller
    {
        private readonly DrinkDAL _drinkDAL = new DrinkDAL();
        private readonly CategoryDAL _categoryDAL = new CategoryDAL();

        // GET: /Drink/
        public IActionResult Index()
        {
            try
            {
                var drinks = _drinkDAL.GetAll();
                return View(drinks);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Không thể tải danh sách đồ uống (kiểm tra kết nối CSDL): {ex.Message}";
                return View(new List<Drink>());
            }
        }

        // GET: /Drink/Create
        public IActionResult Create()
        {
            try
            {
                ViewBag.Categories = _categoryDAL.GetAll();
            }
            catch
            {
                ViewBag.Categories = new List<Category>();
            }
            return View();
        }

        // POST: /Drink/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Drink drink)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int newId = _drinkDAL.Insert(drink);
                    if (newId > 0)
                    {
                        TempData["Success"] = "Thêm đồ uống mới thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Lỗi khi thêm đồ uống: {ex.Message}");
                }
            }

            try
            {
                ViewBag.Categories = _categoryDAL.GetAll();
            }
            catch
            {
                ViewBag.Categories = new List<Category>();
            }
            return View(drink);
        }

        // GET: /Drink/Edit/5
        public IActionResult Edit(int id)
        {
            var drink = _drinkDAL.GetById(id);
            if (drink == null) return NotFound();

            try
            {
                ViewBag.Categories = _categoryDAL.GetAll();
            }
            catch
            {
                ViewBag.Categories = new List<Category>();
            }
            return View(drink);
        }

        // POST: /Drink/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Drink drink)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool ok = _drinkDAL.Update(drink);
                    if (ok)
                    {
                        TempData["Success"] = "Cập nhật thông tin đồ uống thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Lỗi khi cập nhật: {ex.Message}");
                }
            }

            try
            {
                ViewBag.Categories = _categoryDAL.GetAll();
            }
            catch
            {
                ViewBag.Categories = new List<Category>();
            }
            return View(drink);
        }

        // POST: /Drink/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                _drinkDAL.Delete(id);
                TempData["Success"] = "Xóa đồ uống thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi xóa: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
