﻿using System.Linq.Expressions;

namespace Jacmazon_ECommerce.Repositories
{
    public interface ICRUDRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Id</returns>
        Task<int> CreateAsync(TEntity entity);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(int id);

        /// <summary>
        /// 單一查詢
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity?> FindByIdAsync(int id);

        /// <summary>
        /// 多筆查詢
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression);
    }
}
