﻿//using System.Linq.Expressions;
//using System.Reflection;

using AutoMapper;

namespace Jacmazon_ECommerce.Extensions
{
    public static class MappingExpressionExtensionss
    {
        /// <summary>
        /// 忽略所有欄位映射
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDest"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IMappingExpression<TSource, TDest> IgnoreAllUnmapped<TSource, TDest>(this IMappingExpression<TSource, TDest> expression)
        {
            expression.ForAllMembers(opt => opt.Ignore());
            return expression;
        }
    }

    //    public class Mapper
    //    {
    //        /// <summary>
    //        /// 字典缓存，保存的是委托，委托内部是转换的动作
    //        /// </summary>
    //        private static Dictionary<string, object> _Dic = new Dictionary<string, object>();

    //        /// <summary>
    //        /// Expression动态拼接+普通缓存
    //        /// </summary>
    //        /// <typeparam name="TIn"></typeparam>
    //        /// <typeparam name="TOut"></typeparam>
    //        /// <param name="tIn"></param>
    //        /// <returns></returns>
    //        public static TOut Trans<TIn, TOut>(TIn tIn)
    //        {
    //            string key = $"funckey_{typeof(TIn).FullName}_{typeof(TOut).FullName}";
    //            if (!_Dic.ContainsKey(key))
    //            {
    //                ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p");
    //                List<MemberBinding> memberBindingList = new List<MemberBinding>();
    //                foreach (var item in typeof(TOut).GetProperties())
    //                {
    //                    MemberExpression property = Expression.Property(parameterExpression, typeof(TIn).GetProperty(item.Name));
    //                    MemberBinding memberBinding = Expression.Bind(item, property);
    //                    memberBindingList.Add(memberBinding);
    //                }

    //                FieldInfo[] fieldInfos = typeof(TOut).GetFields();
    //                foreach (var item in typeof(TOut).GetFields())
    //                {
    //                    MemberExpression property = Expression.Field(parameterExpression, typeof(TIn).GetField(item.Name));
    //                    MemberBinding memberBinding = Expression.Bind(item, property);
    //                    memberBindingList.Add(memberBinding);
    //                }
    //                MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());
    //                Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new ParameterExpression[]
    //                {
    //                    parameterExpression
    //                });
    //                Func<TIn, TOut> func = lambda.Compile();//拼装是一次性的
    //                _Dic[key] = func;
    //            }
    //            return ((Func<TIn, TOut>)_Dic[key]).Invoke(tIn);
    //        }
    //    }
}

