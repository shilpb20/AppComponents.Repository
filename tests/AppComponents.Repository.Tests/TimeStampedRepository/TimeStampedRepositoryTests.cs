﻿using Microsoft.EntityFrameworkCore;
using TestData = AppComponents.Repository.Tests.TimeStampedRepoTestData;

namespace AppComponents.Repository.Tests
{
    public class TimeStampedRepositoryTests : TimeStampedRepositoryTestsBase, IAsyncLifetime
    {

        [Fact]
        public async Task AddAsync_ReturnsNewlyAddedObject_WhenValidObjectIsAdded()
        {
            //Arrange
            var repository = GetTimeStampedRepository();

            //Act  
            var newItem = TestData.NewItem;
            var countBeforeAddition = _dbContext.TimeStampedMockItems.Count();

            var result = await repository.AddAsync(newItem);

            var newlyAddedObject = await repository.GetAsync(_queryNewItemByName);
            var countAfterAddition = _dbContext.TimeStampedMockItems.Count();

            //Assert
            AssertMockItem(result, newlyAddedObject);
            AssertMockItem(result, newItem);

            Assert.Equal(countBeforeAddition, countAfterAddition - 1);
        }

        [Fact]
        public async Task GetAsync_ReturnsMatchingItem_WhenSingleMatchingDataFound()
        {
            //Arrange
            var repository = GetTimeStampedRepository();

            //Act
            var result = await repository.GetAsync(_queryItemWithId1);

            ////Assert
            AssertMockItem(TestData.MockItems.First(), result);
        }


        [Fact]
        public async Task GetAllAsync_ReturnsAllItems_WhenCalled()
        {
            //Arrange
            var repository = GetTimeStampedRepository();

            //Act
            var allMockItems = await repository?.GetAllAsync();

            //Assert
            AssertMockItems(TestData.MockItems, allMockItems);
        }

        [Fact]
        public async Task GetAll_ReturnsAllItems_WhenCalled()
        {
            //Arrange
            var repository = GetTimeStampedRepository();

            //Act
            var data = await repository?.GetAll();
            var result = await data.ToListAsync();

            //Assert
            AssertMockItems(TestData.MockItems, result);
        }

        [Fact]
        public async Task DeleteAsync_DeletesMatchingObject_WhenFound()
        {
            //Arrange
            var repository = GetTimeStampedRepository();
            var countBeforeDeletion = _dbContext.TimeStampedMockItems.Count();

            //Act  
            var result = await repository.DeleteAsync(TestData.MockItems.First());
            var deletedObject = await repository.GetAsync(_queryItemWithId1);

            var countAfterDeletion = _dbContext.TimeStampedMockItems.Count();


            //Assert
            AssertMockItem(TestData.MockItems.First(), result);
            Assert.Null(deletedObject);
            Assert.Equal(countBeforeDeletion, countAfterDeletion + 1);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsUpdatedValue_OnValidUpdate()
        {
            //Arrange
            await InitializeAsync(TestData.DuplicateMockItems);
            var repository = GetTimeStampedRepository();

            //Act
            //Assert
            var updateObject = await repository.GetAsync(_queryItemForUpdateData);
            AssertMockItem(TestData.DuplicateMockItems.Last(), updateObject);
            updateObject.Name = TestData.UpdateItem.Name;
            updateObject.Value = TestData.UpdateItem.Value;

            Assert.Equal(DateTime.MinValue, updateObject.ModifiedAt);

            var result = await repository.UpdateAsync(updateObject);
            AssertMockItem(TestData.UpdateItem, result);

            Assert.Equal(DateTime.Today, updateObject.ModifiedAt.Date);
        }
    }
}
