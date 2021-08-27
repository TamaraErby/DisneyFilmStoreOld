﻿using DisneyFilmStore.Data;
using DisneyFilmStore.Models.FilmModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisneyFilmStore.Services
{
    public class FilmService
    {
        private readonly Guid _userId;

        public FilmService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateFilm(FilmCreate model)
        {
            var entity =
                new Film()
                {
                    FilmId = model.FilmId,
                    OwnerId = _userId,
                    Title = model.Title,
                };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Films.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<FilmListItem> GetFilms()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Films
                        .Where(e => e.OwnerId == _userId)
                        .Select(
                            e =>
                                new FilmListItem
                                {
                                    FilmId = e.FilmId,
                                    Title = e.Title,
                                }

                        );
                return query.ToArray();
            }
        }

        public FilmDetail GetFilmById(int id)
        {
            var filmService = new FilmService(_userId);
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Films
                        .Single(e => e.FilmId == id && e.OwnerId == _userId);

                return
                    new FilmDetail
                    {
                        Rating = entity.Rating,
                        Genre = entity.Genre,
                        YearReleased = entity.YearReleased,
                        MemberCost = entity.MemberCost,
                        NonMemberCost = entity.NonMemberCost,

                    };
            }
        }


        public bool UpdateFilm(FilmEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Films
                        .Single(e => e.FilmId == model.FilmId && e.OwnerId == _userId);

                entity.Title = model.Title;

                return ctx.SaveChanges() == 1;
            }
        }

        public bool DeleteFilm(int filmId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Films
                        .Single(e => e.FilmId == filmId && e.OwnerId == _userId);

                ctx.Films.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
