using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shootsy.Dtos;
using Shootsy.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Shootsy.Controllers
{
    public class UsersController : Controller
    {
        // GET: UsersController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UsersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //// POST: User/Create
        //[System.Web.Http.HttpPost]
        ////[Consumes(MediaTypeNames.Application.Json)]
        //[SwaggerOperation("Создание пользователя")]
        //[SwaggerResponse(StatusCodes.Status201Created, "Пользователь создан", typeof(int))]
        //public async Task<IActionResult> CreateAsync(
        //    [System.Web.Http.FromBody, BindRequired, SwaggerParameter("Контракт на создание пользователя")]
        //    CreateUserModel model,
        //    CancellationToken cancellationToken = default)
        //{
        //    var user = _mapper.Map<UserDto>(model);
        //    var id = await _userRepository.CreateAsync(user, cancellationToken);

        //    await _userRepository.UpdateAsync(
        //        new UserDto { id = id, Login = id.ToString() }),
        //        new[] { nameof(UserDto.Login) },
        //        cancellationToken);

        //    return StatusCode(201, id);
        //}

        // POST: UsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
