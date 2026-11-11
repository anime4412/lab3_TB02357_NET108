using Microsoft.AspNetCore.Mvc;
using QuanLyThuVien.DAL;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.Controllers
{
    public class BookController : Controller
    {
        private readonly BookDAL _bookDAL = new BookDAL();
        private readonly CategoryDAL _categoryDAL = new CategoryDAL();

        // GET: /Book/
        public IActionResult Index()
        {
            try
            {
                var books = _bookDAL.GetAll();
                return View(books);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Lỗi kết nối CSDL Thư viện: {ex.Message}";
                return View(new List<Book>());
            }
        }

        // GET: /Book/Create
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

        // POST: /Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int newId = _bookDAL.Insert(book);
                    if (newId > 0)
                    {
                        TempData["Success"] = "Thêm sách mới vào thư viện thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Lỗi khi thêm sách: {ex.Message}");
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
            return View(book);
        }

        // GET: /Book/Edit/5
        public IActionResult Edit(int id)
        {
            var book = _bookDAL.GetById(id);
            if (book == null) return NotFound();

            try
            {
                ViewBag.Categories = _categoryDAL.GetAll();
            }
            catch
            {
                ViewBag.Categories = new List<Category>();
            }
            return View(book);
        }

        // POST: /Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool ok = _bookDAL.Update(book);
                    if (ok)
                    {
                        TempData["Success"] = "Cập nhật thông tin sách thành công!";
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
            return View(book);
        }

        // POST: /Book/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                _bookDAL.Delete(id);
                TempData["Success"] = "Xóa sách khỏi hệ thống thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi xóa: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
