using RecruitmentAgencyFinal.Models;
using System;
using System.Web.Mvc;

namespace RecruitmentAgencyFinal.Controllers
{
    public class CandidatesController : Controller
    {
        private DatabaseHelper db = new DatabaseHelper();

        // Проверка доступа
        private bool IsAdminOrManager()
        {
            if (Session["UserId"] == null) return false;
            string role = Session["UserRole"] != null ? Session["UserRole"].ToString() : "";
            return role == "Admin" || role == "Manager";
        }

        public ActionResult Index(string searchTerm, int page = 1)
        {
            if (!IsAdminOrManager())
            {
                return new HttpStatusCodeResult(403, "Доступ запрещён. Только для сотрудников агентства.");
            }

            ViewBag.SearchTerm = searchTerm;
            int pageSize = 5;

            var result = string.IsNullOrEmpty(searchTerm)
                ? db.GetAllCandidatesPaged(page, pageSize)
                : db.SearchCandidates(searchTerm, page, pageSize);

            ViewBag.TotalCount = result.Item2;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((double)result.Item2 / pageSize);

            return View(result.Item1);
        }

        public ActionResult Create()
        {
            if (!IsAdminOrManager())
            {
                return new HttpStatusCodeResult(403, "Доступ запрещён.");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Candidate candidate)
        {
            if (!IsAdminOrManager())
            {
                return new HttpStatusCodeResult(403, "Доступ запрещён.");
            }

            if (ModelState.IsValid)
            {
                candidate.ApplicationDate = DateTime.Now;
                db.AddCandidate(candidate);
                return RedirectToAction("Index");
            }
            return View(candidate);
        }

        public ActionResult Details(int? id)
        {
            if (!IsAdminOrManager())
            {
                return new HttpStatusCodeResult(403, "Доступ запрещён.");
            }

            if (id == null)
                return new HttpStatusCodeResult(400);

            var candidates = db.GetAllCandidates();
            var candidate = candidates.Find(c => c.Id == id);
            if (candidate == null)
                return HttpNotFound();

            return View(candidate);
        }

        public ActionResult Edit(int? id)
        {
            if (!IsAdminOrManager())
            {
                return new HttpStatusCodeResult(403, "Доступ запрещён.");
            }

            if (id == null)
                return new HttpStatusCodeResult(400);

            var candidates = db.GetAllCandidates();
            var candidate = candidates.Find(c => c.Id == id);
            if (candidate == null)
                return HttpNotFound();

            return View(candidate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Candidate candidate)
        {
            if (!IsAdminOrManager())
            {
                return new HttpStatusCodeResult(403, "Доступ запрещён.");
            }

            if (ModelState.IsValid)
            {
                using (var conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                {
                    conn.Open();
                    string sql = "UPDATE Candidates SET FullName = @FullName, Position = @Position, Email = @Email, Phone = @Phone WHERE Id = @Id";
                    using (var cmd = new System.Data.SqlClient.SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", candidate.Id);
                        cmd.Parameters.AddWithValue("@FullName", candidate.FullName);
                        cmd.Parameters.AddWithValue("@Position", candidate.Position);
                        cmd.Parameters.AddWithValue("@Email", candidate.Email);
                        cmd.Parameters.AddWithValue("@Phone", candidate.Phone);
                        cmd.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Index");
            }
            return View(candidate);
        }

        public ActionResult Delete(int? id)
        {
            if (!IsAdminOrManager())
            {
                return new HttpStatusCodeResult(403, "Доступ запрещён.");
            }

            if (id == null)
                return new HttpStatusCodeResult(400);

            var candidates = db.GetAllCandidates();
            var candidate = candidates.Find(c => c.Id == id);
            if (candidate == null)
                return HttpNotFound();

            return View(candidate);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!IsAdminOrManager())
            {
                return new HttpStatusCodeResult(403, "Доступ запрещён.");
            }

            db.DeleteCandidate(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult DeleteCandidateAjax(int id)
        {
            if (!IsAdminOrManager())
            {
                return Json(new { success = false, message = "Доступ запрещён" });
            }

            var result = db.DeleteCandidate(id);
            if (result)
            {
                return Json(new { success = true, message = "Кандидат удален" });
            }
            return Json(new { success = false, message = "Кандидат не найден" });
        }

        [HttpPost]
        public ActionResult ExportToExcel()
        {
            if (!IsAdminOrManager())
            {
                return new HttpStatusCodeResult(403, "Доступ запрещён.");
            }

            try
            {
                var dt = db.GetAllCandidatesForExport();

                if (dt.Rows.Count == 0)
                {
                    TempData["Error"] = "Нет данных для экспорта";
                    return RedirectToAction("Index");
                }

                string html = "<table border='1'>";
                html += "<tr>";
                foreach (System.Data.DataColumn col in dt.Columns)
                {
                    html += "<th>" + col.ColumnName + "</th>";
                }
                html += "</tr>";

                foreach (System.Data.DataRow row in dt.Rows)
                {
                    html += "<tr>";
                    foreach (var item in row.ItemArray)
                    {
                        html += "<td>" + item.ToString() + "</td>";
                    }
                    html += "</tr>";
                }
                html += "</table>";

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=Candidates_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xls");
                Response.ContentType = "application/vnd.ms-excel";
                Response.Charset = "utf-8";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1251");
                Response.Write(html);
                Response.Flush();
                Response.End();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ExportToExcel error: " + ex.Message);
                TempData["Error"] = "Ошибка при экспорте: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}