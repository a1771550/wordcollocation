<?php
/**
 * Created by IntelliJ IDEA.
 * User: Kevin Lau
 * Date: 09/24/2014
 * Time: 11:59
 */
require_once 'conn_db.php';

if(isset($_POST['delete']) && isset($_POST['isbn']))
{
	$isbn = get_post('isbn');
	$query = "delete from classics where isbn='$isbn'";
	if(!mysql_query($query, $db_server))
		echo "Delete failed: $query<br>" . mysql_errno() . "<br><br>";
}

if(isset($_POST['author']) && isset($_POST['title']) && isset($_POST['category']) && isset($_POST['year']) &&
   isset($_POST['isbn'])
)
{
	$author = get_post('author');
	$title = get_post('title');
	$category = get_post('category');
	$year = get_post('year');
	$isbn = get_post('isbn');

	$query="insert into classics values ('$author','$title', '$category','$year','$isbn')";

	if(!mysql_query($query, $db_server))
		echo "Insert failed: $query<br>".mysql_error()."<br><br>";
}

echo <<<_End
<form action="sqltest.php" method="post">
<pre>
	Author      <input type="text" name="author">
	Title       <input type="text" name="title">
	Category    <input type="text" name="category">
	Year        <input type="text" name="year">
	ISBN        <input type="text" name="isbn">

                <input type="submit" value="Add">
</pre>
</form>
_End;

$query = "select * from classics";
$result = mysql_query($query,$db_server);

if(!$result)die("Database access failed: ".mysql_errno());

$rows=mysql_num_rows($result);

for($j=0;$j<$rows;++$j){
	$row=mysql_fetch_row($result);
	echo <<<_end
<pre>
	  Author    $row[0]
	   Title    $row[1]
	Category    $row[2]
	    Year    $row[3]
	    ISBN    $row[4]
</pre>
<form action="sqltest.php" method="post">
	   <input type="hidden" name="delete" value="yes">
	   <input type="hidden" name="isbn" value="$row[4]">
	   <input type="submit" value="Delete">
</form>
_end;
}

mysql_close($db_server);
function get_post($var){
	return mysql_real_escape_string($_POST[$var]);
}
