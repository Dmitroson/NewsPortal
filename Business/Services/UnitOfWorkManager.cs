using Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Business.Services
{
    public static class UnitOfWorkManager
    {
        private static IUnitOfWork unitOfWork;

        public static void SetUnitOfWork(IUnitOfWork unitOfWorkInstance)
        {
            unitOfWork = unitOfWorkInstance;
        }

        public static IUnitOfWork GetUnitOfWork()
        {
            return unitOfWork;
        }
    }
}
