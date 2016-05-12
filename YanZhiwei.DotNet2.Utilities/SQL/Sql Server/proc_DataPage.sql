-- =============================================
-- Create date: <2012-9-12>
-- Description:    <��Ч��ҳ�洢���̣�������Sql2005>
-- Notes:        <�����ֶ�ǿ�ҽ��齨����>
-- sosoft.cnblogs.com
-- =============================================
create Procedure [dbo].[proc_DataPage] 
 @tableName varchar(50),        --����
 @fields varchar(1000) = '*',    --�ֶ���(ȫ���ֶ�Ϊ*)
 @orderField varchar(1000),        --�����ֶ�(����!֧�ֶ��ֶ�)
 @sqlWhere varchar(1000) = Null,--�������(���ü�where)
 @pageSize int,                    --ÿҳ��������¼
 @pageIndex int = 1 ,            --ָ����ǰΪ�ڼ�ҳ
 @totalPage int output            --������ҳ��
as
begin
    Begin Tran --��ʼ����
    Declare @sql nvarchar(4000);
    Declare @totalRecord int;   
    --�����ܼ�¼��
    if (@sqlWhere='' or @sqlWhere=NULL)
        set @sql = 'select @totalRecord = count(*) from ' + @tableName
    else
        set @sql = 'select @totalRecord = count(*) from ' + @tableName + ' where ' + @sqlWhere
    EXEC sp_executesql @sql,N'@totalRecord int OUTPUT',@totalRecord OUTPUT--�����ܼ�¼��       
    --����������
    select @totalPage=CEILING((@totalRecord+0.0)/@PageSize)

    if (@sqlWhere='' or @sqlWhere=NULL)
        set @sql = 'Select * FROM (select ROW_NUMBER() Over(order by ' + @orderField + ') as rowId,' + @fields + ' from ' + @tableName 
    else
        set @sql = 'Select * FROM (select ROW_NUMBER() Over(order by ' + @orderField + ') as rowId,' + @fields + ' from ' + @tableName + ' where ' + @sqlWhere    
        
    --����ҳ��������Χ���
    if @pageIndex<=0 
        Set @pageIndex = 1
    
    if @pageIndex>@totalPage
        Set @pageIndex = @totalPage

     --����ʼ��ͽ�����
    Declare @startRecord int
    Declare @endRecord int
    
    set @startRecord = (@pageIndex-1)*@PageSize + 1
    set @endRecord = @startRecord + @pageSize - 1

    --�����ϳ�sql���
    set @sql = @sql + ') as ' + @tableName + ' where rowId between ' + Convert(varchar(50),@startRecord) + ' and ' +  Convert(varchar(50),@endRecord)
    --print @sql
    Exec(@sql)

--    print @totalRecord 
    If @@Error <> 0
      Begin
        RollBack Tran
        Return -1
      End
     Else
      Begin
        Commit Tran
        --print @totalRecord 
        Return @totalRecord ---���ؼ�¼����
      End   
end