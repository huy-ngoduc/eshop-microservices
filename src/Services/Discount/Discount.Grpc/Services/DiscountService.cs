using System.Threading.Tasks;
using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _repository;
        private readonly ILogger<DiscountService> _logger;
        private readonly IMapper _mapper;

        public DiscountService(IDiscountRepository repository, ILogger<DiscountService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _repository.GetDiscount(request.ProductName);
            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount  with ProductName {request.ProductName} is not found"));
            }
            _logger.LogInformation("Discount is retrieved for ProductName: {productName}, Amount: {amount}", coupon.ProductName, coupon.Amount);

            return _mapper.Map<CouponModel>(coupon);
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            var success = await _repository.CreateDiscount(coupon);
            if (!success)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Failed to create coupon"));
            }

            _logger.LogInformation("Discount is successfully created. ProductName: {productName}", coupon.ProductName);

            return _mapper.Map<CouponModel>(coupon);
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            var success = await _repository.UpdateDiscount(coupon);
            if (!success)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Failed to update coupon"));
            }

            _logger.LogInformation("Discount is successfully updated. ProductName: {productName}", coupon.ProductName);

            return _mapper.Map<CouponModel>(coupon);
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var success = await _repository.DeleteDiscount(request.ProductName);
            if (!success)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Failed to delete coupon"));
            }

            _logger.LogInformation("Discount is successfully deleted. ProductName: {productName}", request.ProductName);

            return new DeleteDiscountResponse() { Success = true };
        }
    }
}
