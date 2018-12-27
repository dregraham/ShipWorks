using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates;
using ShipWorks.Tests.Shared;
using ShipWorks.UI.Dialogs.DefaultPickListTemplate;
using Xunit;

namespace ShipWorks.UI.Tests.Dialogs
{
    public class DefaultPickListTemplateDialogViewModelTest
    {
        private DefaultPickListTemplateDialogViewModel testObject;
        private readonly AutoMock mock;
        
        public DefaultPickListTemplateDialogViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Constructor_LoadsTemplatesFromPickListFolder_WhenPickListFolderExists()
        {
            var pickListTemplate = new TemplateEntity { Name = "Pick List Template" };
            var templateThatShouldNotShowUp = new TemplateEntity { Name = "Report Template" };
            var pickListFolder = new TemplateFolderEntity
            {
                Name = "Pick Lists", TemplateFolderID = 1000, Templates = { pickListTemplate }
            };
            var reportFolder = new TemplateFolderEntity
            {
                Name = "Reports", TemplateFolderID = 2000, Templates = { templateThatShouldNotShowUp }
            };

            pickListTemplate.IsDirty = false;
            templateThatShouldNotShowUp.IsDirty = false;
            pickListFolder.IsDirty = false;
            reportFolder.IsDirty = false;
            
            IEnumerable<TemplateFolderEntity> folders = new[] { pickListFolder, reportFolder };
            IEnumerable<TemplateEntity> templates = new[] { pickListTemplate, templateThatShouldNotShowUp };
            
            TemplateTree tree = TemplateTree.CreateFrom(folders, templates, null);
            mock.Mock<ITemplateManager>().Setup(x => x.Tree).Returns(tree);
            testObject = mock.Create<DefaultPickListTemplateDialogViewModel>();

            Assert.Equal(pickListTemplate.TemplateID, testObject.PickListTemplates.SingleOrDefault().TemplateID);
        }
        
        [Fact]
        public void Constructor_LoadsTemplatesFromReportsFolder_WhenPickListFolderDoesNotExist()
        {
            var fooFolder = new TemplateFolderEntity { Name = "Foo", TemplateFolderID = 1000, IsDirty = false };
            var reportFolder = new TemplateFolderEntity { Name = "Reports", TemplateFolderID = 2000, IsDirty = false };
            
            var templateThatShouldNotShowUp = new TemplateEntity
            {
                Name = "Foo Template",
                IsDirty = false
            };
            var pickListTemplate = new TemplateEntity
            {
                ParentFolder = reportFolder,
                ParentFolderID = 2000,
                Name = "Pick List Template From Reports Folder",
                IsDirty = false
            };

            IEnumerable<TemplateFolderEntity> folders = new[] { reportFolder, fooFolder };
            IEnumerable<TemplateEntity> templates = new[] { pickListTemplate, templateThatShouldNotShowUp };
            
            TemplateTree tree = TemplateTree.CreateFrom(folders, templates, null);
            mock.Mock<ITemplateManager>().Setup(x => x.Tree).Returns(tree);
            testObject = mock.Create<DefaultPickListTemplateDialogViewModel>();

            Assert.Equal(pickListTemplate.TemplateID, testObject.PickListTemplates.SingleOrDefault().TemplateID);
        }

        [Fact]
        public void SupportArticleLink_IsInitializedWithCorrectUrl()
        {
            IEnumerable<TemplateFolderEntity> folders = new TemplateFolderEntity[0];
            IEnumerable<TemplateEntity> templates = new TemplateEntity[0];
            TemplateTree tree = TemplateTree.CreateFrom(folders, templates, null);
            mock.Mock<ITemplateManager>().Setup(x => x.Tree).Returns(tree);

            testObject = mock.Create<DefaultPickListTemplateDialogViewModel>();
            Assert.Equal("http://support.shipworks.com/", testObject.SupportArticleLink.AbsoluteUri);
        }

        [Fact]
        public void SavePickListTemplateCommand_CanExecute_WhenSelectedPickListTemplateIsNotNull()
        {
            IEnumerable<TemplateFolderEntity> folders = new TemplateFolderEntity[0];
            IEnumerable<TemplateEntity> templates = new TemplateEntity[0];
            TemplateTree tree = TemplateTree.CreateFrom(folders, templates, null);
            mock.Mock<ITemplateManager>().Setup(x => x.Tree).Returns(tree);

            testObject = mock.Create<DefaultPickListTemplateDialogViewModel>();
            testObject.SelectedPickListTemplate = new TemplateEntity();
            
            Assert.True(testObject.SavePickListTemplateCommand.CanExecute(null));
        }
        
        [Fact]
        public void SavePickListTemplateCommand_CanNotExecute_WhenSelectedPickListTemplateIsNull()
        {
            IEnumerable<TemplateFolderEntity> folders = new TemplateFolderEntity[0];
            IEnumerable<TemplateEntity> templates = new TemplateEntity[0];
            TemplateTree tree = TemplateTree.CreateFrom(folders, templates, null);
            mock.Mock<ITemplateManager>().Setup(x => x.Tree).Returns(tree);

            testObject = mock.Create<DefaultPickListTemplateDialogViewModel>();
            testObject.SelectedPickListTemplate = null;
            
            Assert.False(testObject.SavePickListTemplateCommand.CanExecute(null));
        }

        [Fact]
        public void SavePickListTemplate_CallsUpdateConfiguration()
        {
            IEnumerable<TemplateFolderEntity> folders = new TemplateFolderEntity[0];
            IEnumerable<TemplateEntity> templates = new TemplateEntity[0];
            TemplateTree tree = TemplateTree.CreateFrom(folders, templates, null);
            mock.Mock<ITemplateManager>().Setup(x => x.Tree).Returns(tree);

            var configData = mock.Mock<IConfigurationData>();
            testObject = mock.Create<DefaultPickListTemplateDialogViewModel>();
            testObject.SelectedPickListTemplate = new TemplateEntity(1);
            testObject.SavePickListTemplateCommand.Execute(null);
            
            configData.Verify(x => x.UpdateConfiguration(It.IsAny<Action<ConfigurationEntity>>()));
        }
    }
}